using System.ComponentModel.DataAnnotations;


namespace ProjectManager.Api.Models
{
    public class ProjectTask
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public required string Title { get; set; } = null!;

        public DateTime? DueDate { get; set; }

        public bool IsCompleted { get; set; } = false;

        // Relationship
        public Guid ProjectId { get; set; }
        public required Project Project { get; set; }
    }
}