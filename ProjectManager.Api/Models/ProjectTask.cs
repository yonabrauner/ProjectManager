using System.ComponentModel.DataAnnotations;


namespace ProjectManager.Api.Models
{
    public class ProjectTask
    {
        public int Id { get; set; }

        [Required]
        public required string Title { get; set; } = null!;

        public DateTime? DueDate { get; set; }

        public bool IsCompleted { get; set; } = false;

        // Relationship
        public int ProjectId { get; set; }
        public required Project Project { get; set; }
    }
}