using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ProjectManager.Api.Services;
using ProjectManager.Api.DTOs;
using ProjectManager.Api.Extensions;

namespace ProjectManager.API.Controllers
{
    [ApiController]
    [Route("api/projects/{projectId}/tasks")]
    [Authorize] // Require authentication via JWT
    public class ProjectTaskController : ControllerBase
    {
        private readonly IProjectTaskService _taskService;

        public ProjectTaskController(IProjectTaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask(Guid projectId, [FromBody] TaskCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Ensure route projectId matches dto.ProjectId (if provided)
            if (dto.ProjectId != projectId)
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
        [HttpPut("{taskId}")]
        public async Task<IActionResult> UpdateTask(Guid projectId, Guid taskId, [FromBody] TaskUpdateDto dto)
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
        [HttpDelete("{taskId}")]
        public async Task<IActionResult> DeleteTask(Guid projectId, Guid taskId)
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
        public async Task<IActionResult> GetTaskById(Guid projectId, Guid taskId)
        {
            try
            {
                var userId = ClaimsPrincipalExtensions.GetUserId(User);
                var response = await _taskService.GetTaskByIdAsync(taskId, userId);

                if (response == null || response.Data!.ProjectId != projectId)
                    return NotFound(new { error = "Task not found or access denied." });

                return Ok(response.Data);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetTasksByProject(
            Guid projectId,
            [FromQuery] bool? completed = null,
            [FromQuery] string? sort = null
        )
        {
            try
            {
                var userId = ClaimsPrincipalExtensions.GetUserId(User);
                var response = await _taskService.GetTasksByProjectIdAsync(projectId, userId, completed, sort);
                return Ok(response.Data);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
