using ProjectManager.Api.DTOs;
using ProjectManager.Api.Common;

namespace ProjectManager.Api.Services.Interfaces
{
    public interface IAuthService
    {
        /// <summary>
        /// Register a new user.
        /// </summary>
        /// <param name="dto">Registration data (email, password, etc.)</param>
        /// <returns>A service result indicating success or failure.</returns>
        Task<ServiceResult<AuthResponseDto>> RegisterAsync(RegisterDto dto);

        /// <summary>
        /// Login an existing user.
        /// </summary>
        /// <param name="dto">Login credentials (email + password).</param>
        /// <returns>A service result with a JWT token if successful.</returns>
        Task<ServiceResult<AuthResponseDto>> LoginAsync(LoginDto dto);
    }
}