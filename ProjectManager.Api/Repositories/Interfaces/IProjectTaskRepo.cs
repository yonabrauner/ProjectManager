using ProjectManager.Api.Models;


public interface IProjectTaskRepository
{
    Task<IEnumerable<ProjectTask>> GetTasksByProjectIdAsync(int projectId, bool? completed = null, string? sort = null);
    Task<ProjectTask?> GetByIdAsync(int taskId);
    Task AddAsync(ProjectTask task);
    Task UpdateAsync(ProjectTask task); // Optional: for completeness
    Task DeleteAsync(ProjectTask task);
}