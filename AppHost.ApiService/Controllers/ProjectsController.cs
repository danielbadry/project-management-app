namespace AppHost.ApiService.Controllers;

using Microsoft.AspNetCore.Mvc;
using AppHost.ApiService.Services.ProjectManagement;
using Shared.Dtos.ProjectForm;

[ApiController]
[Route("api/projects")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectsService _projectService;

    public ProjectsController(IProjectsService projectsService)
    {
        _projectService = projectsService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateProject(
        [FromBody] ProjectFormRequestDto request)
    {
        var result = await _projectService.CreateProjectAsync(request);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }

        return Ok(result);
    }
}