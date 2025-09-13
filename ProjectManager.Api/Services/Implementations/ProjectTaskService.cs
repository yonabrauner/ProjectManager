using Microsoft.EntityFrameworkCore;
using ProjectManager.Api.Data;
using ProjectManager.Api.DTOs;
using ProjectManager.Api.Models;
using ProjectManager.Api.Common;
using ProjectManager.Api.Repositories.Interfaces;


namespace ProjectManager.Api.Services
{
    public class ProjectTaskService : IProjectTaskService
    {
        private readonly AppDbContext _context;
        private readonly IProjectTaskRepository _projectTaskRepository;
        private readonly IProjectRepository _projectRepository;

        public ProjectTaskService(AppDbContext context, IProjectTaskRepository projectTaskRepository, IProjectRepository projectRepository)
        {
            _context = context;
            _projectTaskRepository = projectTaskRepository;
            _projectRepository = projectRepository;
        }

        public async Task<ServiceResult<IEnumerable<TaskResponseDto>>> GetTasksByProjectIdAsync(Guid projectId, Guid userId, bool? completed = null, string? sort = null)
        {
            var query = _projectTaskRepository.GetQueryableByProjectId(projectId);
            query = query.Where(t => t.Project != null && t.Project.UserId == userId);

            // filtering
            if (completed.HasValue)
                query = query.Where(t => t.IsCompleted == completed.Value);

            // sorting
            query = sort switch
            {
                "duedate_asc" => query.OrderBy(t => t.DueDate),
                "duedate_desc" => query.OrderByDescending(t => t.DueDate),
                "title_asc" => query.OrderBy(t => t.Title),
                "title_desc" => query.OrderByDescending(t => t.Title),
                _ => query.OrderBy(t => t.Id)
            };

            var tasks = await query.ToListAsync();

            var response = tasks.Select(t => new TaskResponseDto
            {
                Id = t.Id,
                Title = t.Title,
                IsCompleted = t.IsCompleted,
                DueDate = t.DueDate,
                ProjectId = t.ProjectId
            });
            return ServiceResult<IEnumerable<TaskResponseDto>>.Ok(response, "Successfully got tasks.");
        }

        public async Task<ServiceResult<TaskResponseDto?>> GetTaskByIdAsync(Guid taskId, Guid userId)
        {
            // Find the task including its project
            var task = await _projectTaskRepository.GetByIdAsync(taskId);

            if (task == null)
                throw new ApplicationException($"Task with ID {taskId} not found.");

            if (task.Project == null || task.Project.UserId != userId)
                throw new ApplicationException($"Task with ID {taskId} not found or access denied.");

            var response = new TaskResponseDto
            {
                Id = task.Id,
                Title = task.Title,
                IsCompleted = task.IsCompleted,
                DueDate = task.DueDate,
                ProjectId = task.ProjectId,
            };
            return ServiceResult<TaskResponseDto?>.Ok(response, "Successfully got task.");
        }

        public async Task<ServiceResult<TaskResponseDto>> CreateTaskAsync(TaskCreateDto dto, Guid userId)
        {
            // ensure project belongs to this user
            var project = await _projectRepository.GetByIdForUserAsync(dto.ProjectId, userId);

            if (project == null)
                throw new ApplicationException("Project not found or does not belong to user.");

            if (string.IsNullOrWhiteSpace(dto.Title))
                return ServiceResult<TaskResponseDto>.Fail("Task title is required.");

            var task = new ProjectTask
            {
                Title = dto.Title,
                IsCompleted = false,
                DueDate = dto.DueDate,
                ProjectId = dto.ProjectId,
                Project = project,
            };

            _context.ProjectTasks.Add(task);

            await _projectTaskRepository.AddAsync(task);
            await _projectTaskRepository.SaveChangesAsync();

            var response = new TaskResponseDto
            {
                Id = task.Id,
                Title = task.Title,
                IsCompleted = task.IsCompleted,
                DueDate = task.DueDate,
                ProjectId = task.ProjectId,
            };
            return ServiceResult<TaskResponseDto>.Ok(response, "Successfully added task.");
 
        }

        public async Task<ServiceResult<TaskResponseDto?>> UpdateTaskAsync(Guid taskId, TaskUpdateDto dto, Guid userId)
        {
            var task = await _context.ProjectTasks
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.Id == taskId && t.Project != null && t.Project.UserId == userId);

            if (task == null)
                throw new ApplicationException("Error: Failed to find Task to update.");

            task.Title = dto.Title ?? task.Title;
            task.IsCompleted = dto.IsCompleted ?? task.IsCompleted;
            task.DueDate = dto.DueDate ?? task.DueDate;

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

            var response = new TaskResponseDto
            {
                Id = task.Id,
                Title = task.Title,
                IsCompleted = task.IsCompleted,
                DueDate = task.DueDate,
                ProjectId = task.ProjectId,
            };
            return ServiceResult<TaskResponseDto?>.Ok(response, "Successfully updated task.");

        }

        public async Task<ServiceResult<bool>> DeleteTaskAsync(Guid taskId, Guid userId)
        {
            var task = await _context.ProjectTasks
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.Id == taskId && t.Project.UserId == userId);

            if (task == null)
                throw new ApplicationException("Error: Failed to find Task to delete.");

            _context.ProjectTasks.Remove(task);
            
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
            
            return ServiceResult<bool>.Ok(true, "Successfully deleted task.");
        }
    }
}
