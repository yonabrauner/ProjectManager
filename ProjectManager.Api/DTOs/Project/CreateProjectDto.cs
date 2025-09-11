using System.ComponentModel.DataAnnotations;

namespace ProjectManager.Api.DTOs
{
    public class ProjectCreateDto
    {
        [Required, StringLength(maximumLength: 100, MinimumLength = 3, ErrorMessage = "Project title must be between 3 and 100 characters.")]
        public string Title { get; set; } = null!;

        [StringLength(500, ErrorMessage = "Description can be at most 500 characters.")]
        public string? Description { get; set; }
    }
}
