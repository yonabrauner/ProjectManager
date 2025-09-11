namespace ProjectManager.Api.DTOs
{
    public class TaskResponseDto
    {
        public int Id { get; set; }
        public required string Title { get; set; } = null!;
        public bool IsCompleted { get; set; }
        public DateTime? DueDate { get; set; }
        public int ProjectId { get; set; }
    }
}
