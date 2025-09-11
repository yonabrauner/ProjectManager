using ProjectManager.Api.DTOs;
using ProjectManager.Api.Common;

namespace ProjectManager.Api.Services
{
    public interface IProjectTaskService
    {
        /// <summary>
        /// Get all tasks from a specific project. ensuring the project belongs to the user.
        /// </summary>
        Task<ServiceResult<IEnumerable<TaskResponseDto>>> GetTasksByProjectIdAsync(int projectId, int userId, bool? completed = null, string? sort = null);

        /// <summary>
        /// Get specific task owned by a specific user.
        /// </summary>
        Task<ServiceResult<TaskResponseDto?>> GetTaskByIdAsync(int taskId, int userId);

        /// <summary>
        /// Create a new task in a specific project, ensuring the project belongs to the user.
        /// </summary>
        Task<ServiceResult<TaskResponseDto>> CreateTaskAsync(TaskCreateDto dto, int userId);

        /// <summary>
        /// Update a specific task, ensuring it is in a project that belongs to the user.
        /// </summary>
        Task<ServiceResult<TaskResponseDto?>> UpdateTaskAsync(int taskId, TaskUpdateDto dto, int userId);

        /// <summary>
        /// Delete a specific task, ensuring it is in a project that belongs to the user.
        /// </summary>
        Task<ServiceResult<bool>> DeleteTaskAsync(int taskId, int userId);
    }
}