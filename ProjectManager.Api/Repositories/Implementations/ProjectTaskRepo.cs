using Microsoft.EntityFrameworkCore;
using ProjectManager.Api.Data;
using ProjectManager.Api.Models;
using ProjectManager.Api.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.Api.Repositories.Implementations
{
    public class ProjectTaskRepository : IProjectTaskRepository
    {
        private readonly AppDbContext _context;

        public ProjectTaskRepository(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<ProjectTask> GetQueryableByProjectId(Guid projectId)
        {
            return _context.ProjectTasks
                .Include(t => t.Project)
                .Where(t => t.ProjectId == projectId);
        }

        public async Task<ProjectTask?> GetByIdAsync(Guid taskId)
        {
            return await _context.ProjectTasks
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.Id == taskId);
        }

        public async Task AddAsync(ProjectTask task)
        {
            await _context.ProjectTasks.AddAsync(task);
        }

        public void Remove(ProjectTask task)
        {
            _context.ProjectTasks.Remove(task);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
