using Microsoft.EntityFrameworkCore;
using ProjectManager.Api.Helpers;
using ProjectManager.Api.Data;
using ProjectManager.Api.DTOs;
using ProjectManager.Api.Models;
using ProjectManager.Api.Services.Interfaces;
using ProjectManager.Api.Common;


namespace ProjectManager.Api.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<ServiceResult<AuthResponseDto>> RegisterAsync(RegisterDto dto)
        {
            // Check if username already exists
            if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
                return ServiceResult<AuthResponseDto>.Fail("Username already registered.");

            // double check that password is semi-valid
            if (string.IsNullOrWhiteSpace(dto.Password))
                return ServiceResult<AuthResponseDto>.Fail("Invalid Password.");
                
            // Hash the password
            var hashedPassword = AuthHelper.HashPassword(dto.Password);

            var user = new User
            {
                Username = dto.Username,
                PasswordHash = hashedPassword,
            };

            _context.Users.Add(user);

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

            var response = new AuthResponseDto
            {
                Token = AuthHelper.GenerateJwtToken(user, _configuration),
                UserId = user.Id,
                Username = user.Username,
            };

            return ServiceResult<AuthResponseDto>.Ok(response, "User registered successfully.");
        }

        public async Task<ServiceResult<AuthResponseDto>> LoginAsync(LoginDto dto)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
                if (user == null || !AuthHelper.VerifyPassword(dto.Password, user.PasswordHash))
                    return ServiceResult<AuthResponseDto>.Fail("invalid credentials.");

                var token = AuthHelper.GenerateJwtToken(user, _configuration);
                
                var response = new AuthResponseDto
                {
                    Token = token,
                    UserId = user.Id,
                    Username = user.Username,
                };
                return ServiceResult<AuthResponseDto>.Ok(response, "Login successful.");
            }
            catch (ArgumentNullException ex)
            {
                throw new ApplicationException("Failed to login due to missing input.", ex);
            }
            catch (OperationCanceledException ex)
            {
                throw new ApplicationException("Login was canceled. Please try again.", ex);
            }
        }
    }
}