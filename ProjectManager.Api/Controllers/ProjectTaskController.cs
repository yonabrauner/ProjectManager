using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ProjectManager.Api.Services;
using ProjectManager.Api.DTOs;
using ProjectManager.Api.Extensions;

namespace ProjectManager.API.Controllers
{
    [ApiController]
    [Route("api/projects/{projectId}/[controller]s")]
    [Authorize] // Require authentication via JWT
    public class ProjectTaskController : ControllerBase
    {
        private readonly IProjectTaskService _taskService;

        public ProjectTaskController(IProjectTaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost("projects/{projectId}/tasks")]
        public async Task<IActionResult> CreateTask(int projectId, [FromBody] TaskCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Ensure route projectId matches dto.ProjectId (if provided)
            if (dto.ProjectId != 0 && dto.ProjectId != projectId)
                return BadRequest(new { error = "ProjectId in body does not match ProjectId in route." });

            try
            {
                var userId = ClaimsPrincipalExtensions.GetUserId(User);
                var response = await _taskService.CreateTaskAsync(dto, userId);
                return StatusCode(StatusCodes.Status201Created, response.Data);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // PUT /api/projects/{projectId}/tasks/{taskId}
        [HttpPut("tasks/{taskId}")]
        public async Task<IActionResult> UpdateTask(int projectId, int taskId, [FromBody] TaskUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var userId = ClaimsPrincipalExtensions.GetUserId(User);
                var response = await _taskService.UpdateTaskAsync(taskId, dto, userId);

                if (response == null || response.Data!.ProjectId != projectId)
                    return NotFound(new { error = "Task not found or access denied." });

                return StatusCode(StatusCodes.Status201Created, response.Data);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // DELETE /api/projects/{projectId}/tasks/{taskId}
        [HttpDelete("tasks/{taskId}")]
        public async Task<IActionResult> DeleteTask(int projectId, int taskId)
        {
            try
            {
                var userId = ClaimsPrincipalExtensions.GetUserId(User);
                var response = await _taskService.DeleteTaskAsync(taskId, userId);

                if (!response.Data)
                    return NotFound(new { error = "Task not found or access denied." });

                return NoContent();
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // GET /api/projects/{projectId}/tasks/{taskId}
        [HttpGet("{taskId}")]
        public async Task<IActionResult> GetTaskById(int projectId, int taskId)
        {
            try
            {
                var userId = ClaimsPrincipalExtensions.GetUserId(User);
                var response = await _taskService.GetTaskByIdAsync(taskId, userId);

                if (response == null || response.Data!.ProjectId != projectId)
                    return NotFound(new { error = "Task not found or access denied." });

                return Ok(response);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("projects/{projectId}/tasks")]
        public async Task<IActionResult> GetTasksByProject(
            int projectId,
            [FromQuery] bool? completed = null,
            [FromQuery] string? sort = null
        )
        {
            try
            {
                var userId = ClaimsPrincipalExtensions.GetUserId(User);
                var response = await _taskService.GetTasksByProjectIdAsync(projectId, userId, completed, sort);
                return Ok(response);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
