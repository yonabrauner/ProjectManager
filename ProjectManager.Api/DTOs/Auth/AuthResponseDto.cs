namespace ProjectManager.Api.DTOs
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = null!; // JWT token
        public Guid UserId { get; set; }             
        public string Username { get; set; } = null!;
        
    }
}