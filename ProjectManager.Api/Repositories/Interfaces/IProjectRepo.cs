using ProjectManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManager.Api.Repositories.Interfaces
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetByUserIdAsync(Guid userId);
        Task<Project?> GetByIdAsync(Guid projectId);
        Task<Project?> GetByIdForUserAsync(Guid projectId, Guid userId);
        Task AddAsync(Project project);
        void Remove(Project project);
        Task SaveChangesAsync();
    }
}
