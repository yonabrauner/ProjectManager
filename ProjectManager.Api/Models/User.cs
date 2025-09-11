using System.ComponentModel.DataAnnotations;


namespace ProjectManager.Api.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public required string Username { get; set; }

        [Required]
        public required string PasswordHash { get; set; }
        public ICollection<Project> Projects { get; set; } = new List<Project>();
    }
}