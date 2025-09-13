using System.ComponentModel.DataAnnotations;


namespace ProjectManager.Api.DTOs
{
    public class TaskCreateDto
    {
        [Required, StringLength(200, MinimumLength = 1, ErrorMessage = "Task title must be between 1 and 200 characters.")]
        public string? Title { get; set; }

        public DateTime? DueDate { get; set; }


        [Required(ErrorMessage = "ProjectID is required.")]
        public Guid ProjectId { get; set; }

        public bool IsCompleted { get; set; } = false;
    }
}