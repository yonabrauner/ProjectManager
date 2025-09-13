using System.ComponentModel.DataAnnotations;


namespace ProjectManager.Api.Models
{
    public class Project
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, StringLength(maximumLength: 100, MinimumLength = 3)]
        public required string Title { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<ProjectTask> Tasks { get; set; } = new List<ProjectTask>();

        // Relationship
        [Required]
        public required Guid UserId { get; set; }
        public User? User { get; set; }
    }
}