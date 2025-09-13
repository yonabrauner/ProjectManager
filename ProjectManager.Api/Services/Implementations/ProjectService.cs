using Microsoft.EntityFrameworkCore;
using ProjectManager.Api.Data;
using ProjectManager.Api.DTOs;
using ProjectManager.Api.Models;
using ProjectManager.Api.Common;
namespace ProjectManager.Api.Services
{
    public class ProjectService : IProjectService
    {
        private readonly AppDbContext _context;

        public ProjectService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<IEnumerable<ProjectResponseDto>>> GetUserProjectsAsync(Guid userId)
        {
            // Check if user exists
            var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
            if (!userExists)
                throw new ApplicationException($"User with ID {userId} not found.");

            // Get all projects for the user, include tasks
            var projects = await _context.Projects
                .Where(p => p.UserId == userId)
                .Include(p => p.Tasks)
                .ToListAsync();

            var response = projects.Select(p => new ProjectResponseDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                CreatedAt = p.CreatedAt
            }).ToList();

            return ServiceResult<IEnumerable<ProjectResponseDto>>.Ok(response, "Got projects successfully.");
        }

        public async Task<ServiceResult<ProjectResponseDto>> CreateProjectAsync(ProjectCreateDto dto, Guid userId)
        {
            // Check if user exists
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new ApplicationException($"User with ID {userId} not found.");

            if (string.IsNullOrWhiteSpace(dto.Title))
                return ServiceResult<ProjectResponseDto>.Fail("Invalid project title.");

            var project = new Project
            {
                Title = dto.Title!,
                Description = dto.Description ?? string.Empty,
                UserId = userId,
            };

            _context.Projects.Add(project);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new ApplicationException("Failed to update DB.", ex);
            }
            catch (OperationCanceledException ex)
            {
                throw new ApplicationException("Operation canceled.", ex);
            }

            var response = new ProjectResponseDto
            {
                Id = project.Id,
                Title = project.Title,
                Description = project.Description,
                CreatedAt = project.CreatedAt
            };
            return ServiceResult<ProjectResponseDto>.Ok(response, "Successfully created project.");
        }

        public async Task<ServiceResult<ProjectResponseDto?>> GetProjectByIdAsync(Guid projectId, Guid userId)
        {
            var project = await _context.Projects
                .Include(p => p.Tasks)
                .FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null)
                throw new ApplicationException($"Project with ID {projectId} not found.");

            if (project.UserId != userId)
                throw new ApplicationException($"Project with ID {projectId} not found or access denied.");

            var response = new ProjectResponseDto
            {
                Id = project.Id,
                Title = project.Title,
                Description = project.Description,
                CreatedAt = project.CreatedAt
            };
            return ServiceResult<ProjectResponseDto?>.Ok(response, "Successfully got project.");
        }

        public async Task<ServiceResult<bool>> DeleteProjectAsync(Guid projectId, Guid userId)
        {
            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == projectId && p.UserId == userId);

            if (project == null)
                throw new ApplicationException("Error: Failed to find Project to delete.");

            _context.Projects.Remove(project);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new ApplicationException("Failed to update DB.", ex);
            }
            catch (OperationCanceledException ex)
            {
                throw new ApplicationException("Operation canceled.", ex);
            }

            return ServiceResult<bool>.Ok(true, "Successfully deleted project.");
        }
    }
}