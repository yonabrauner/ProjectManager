using Microsoft.EntityFrameworkCore;
using ProjectManager.Api.Data;
using ProjectManager.Api.DTOs;
using ProjectManager.Api.Models;
using ProjectManager.Api.Common;
using ProjectManager.Api.Repositories.Interfaces;


namespace ProjectManager.Api.Services
{
    public class ProjectService : IProjectService
    {
        private readonly AppDbContext _context;
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;
        public ProjectService(AppDbContext context, IProjectRepository projectRepository, IUserRepository userRepository)
        {
            _context = context;
            _projectRepository = projectRepository;
            _userRepository = userRepository;
        }

        public async Task<ServiceResult<IEnumerable<ProjectResponseDto>>> GetUserProjectsAsync(Guid userId)
        {
            // Check if user exists
            var userExists = await _userRepository.GetByIdAsync(userId);

            // Get all projects for the user, include tasks
            var projects = await _projectRepository.GetByUserIdAsync(userId);

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
            Console.WriteLine($"Received: {System.Text.Json.JsonSerializer.Serialize(dto)}");
            // Check if user exists
            var user = await _userRepository.GetByIdAsync(userId);

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

            await _projectRepository.AddAsync(project);
            await _projectRepository.SaveChangesAsync();

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
            var project = await _projectRepository.GetByIdAsync(projectId);

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
            var project = await _projectRepository.GetByIdAsync(projectId);
            if (project == null || project.UserId != userId) 
                throw new ApplicationException("Project not found or access denied.");

            _projectRepository.Remove(project);
            await _projectRepository.SaveChangesAsync();

            return ServiceResult<bool>.Ok(true, "Successfully deleted project.");
        }
    }
}