using ProjectManager.Api.Models;


namespace ProjectManager.Api.Repositories.Interfaces
{
    public interface IProjectTaskRepository
    {
        IQueryable<ProjectTask> GetQueryableByProjectId(Guid projectId);
        Task<ProjectTask?> GetByIdAsync(Guid taskId);
        Task AddAsync(ProjectTask task);
        void Remove(ProjectTask task);
        Task SaveChangesAsync();
    }
}