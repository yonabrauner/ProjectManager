namespace ProjectManager.Api.DTOs
{
    public class TaskResponseDto
    {
        public Guid Id { get; set; }
        public required string Title { get; set; } = null!;
        public bool IsCompleted { get; set; }
        public DateTime? DueDate { get; set; }
        public Guid ProjectId { get; set; }
    }
}
