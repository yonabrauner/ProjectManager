using ProjectManager.Api.DTOs;
using ProjectManager.Api.Common;

namespace ProjectManager.Api.Services
{
    public interface IProjectService
    {
        /// <summary>
        /// Get all projects owned by a specific user.
        /// </summary>
        Task<ServiceResult<IEnumerable<ProjectResponseDto>>> GetUserProjectsAsync(int userId);

        /// <summary>
        /// Create a new project for a specific user.
        /// </summary>
        Task<ServiceResult<ProjectResponseDto>> CreateProjectAsync(ProjectCreateDto dto, int userId);

        /// <summary>
        /// Get a project by ID, ensuring it belongs to the user.
        /// </summary>
        Task<ServiceResult<ProjectResponseDto?>> GetProjectByIdAsync(int projectId, int userId);

        /// <summary>
        /// Delete a project by ID, ensuring it belongs to the user.
        /// Returns true if successful, false if not found or unauthorized.
        /// </summary>
        Task<ServiceResult<bool>> DeleteProjectAsync(int projectId, int userId);
    }
}