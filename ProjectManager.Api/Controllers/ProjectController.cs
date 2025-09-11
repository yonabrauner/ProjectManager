using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ProjectManager.Api.Services;
using ProjectManager.Api.DTOs;
using ProjectManager.Api.Extensions;

namespace ProjectManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    [Authorize]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var userId = ClaimsPrincipalExtensions.GetUserId(User);
                var response = await _projectService.GetUserProjectsAsync(userId);
                return StatusCode(StatusCodes.Status200OK, response.Data);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProjectCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var userId = ClaimsPrincipalExtensions.GetUserId(User);
                var response = await _projectService.CreateProjectAsync(dto, userId);
                return StatusCode(StatusCodes.Status201Created, response.Data);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var userId = ClaimsPrincipalExtensions.GetUserId(User);
                var response = await _projectService.GetProjectByIdAsync(id, userId);
                if (response == null)
                    return NotFound(new { error = "Project not found or access denied." });

                return StatusCode(StatusCodes.Status200OK, response.Data);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var userId = ClaimsPrincipalExtensions.GetUserId(User);
                var response = await _projectService.DeleteProjectAsync(id, userId);
                if (!response.Data)
                    return NotFound(new { error = "Project not found or access denied." });

                return NoContent();
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}