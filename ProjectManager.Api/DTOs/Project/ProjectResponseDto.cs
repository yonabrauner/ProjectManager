namespace ProjectManager.Api.DTOs
{
    public class ProjectResponseDto
    {
        public Guid Id { get; set; }

        public required string Title { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; }

        // // Include tasks if needed
        // public List<ProjectTaskResponseDto> Tasks { get; set; } = new();
    }
}
