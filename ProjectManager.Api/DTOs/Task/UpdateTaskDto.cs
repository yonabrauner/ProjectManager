namespace ProjectManager.Api.DTOs
{
    public class TaskUpdateDto
    {
        // Optional fields – since not every property needs to be updated
        public string? Title { get; set; }
        public bool? IsCompleted { get; set; }
        public DateTime? DueDate { get; set; }
    }
}