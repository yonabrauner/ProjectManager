using ProjectManager.Api.Models;
using System;
using System.Threading.Tasks;

namespace ProjectManager.Api.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByUsernameAsync(string username);
        Task AddAsync(User user);
        Task<bool> ExistsByUsernameAsync(string username);
        Task SaveChangesAsync();
    }
}
