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
    public class ProjectRepository : IProjectRepository
    {
        private readonly AppDbContext _context;

        public ProjectRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Project>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Projects
                .Where(p => p.UserId == userId)
                .Include(p => p.Tasks)
                .ToListAsync();
        }

        public async Task<Project?> GetByIdForUserAsync(Guid projectId, Guid userId)
        {
            return await _context.Projects
                .Include(p => p.Tasks) // optional, only if you need tasks
                .FirstOrDefaultAsync(p => p.Id == projectId && p.UserId == userId);
        }


        public async Task<Project?> GetByIdAsync(Guid projectId)
        {
            return await _context.Projects
                .Include(p => p.Tasks)
                .FirstOrDefaultAsync(p => p.Id == projectId);
        }

        public async Task AddAsync(Project project)
        {
            await _context.Projects.AddAsync(project);
        }

        public void Remove(Project project)
        {
            _context.Projects.Remove(project);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
