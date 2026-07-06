namespace AppHost.ApiService.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AppHost.ApiService.Services.ProjectManagement;
using Shared.Dtos.ProjectForm;

[ApiController]
[Authorize]
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

    [HttpGet]
    public async Task<IActionResult> GetProjectList()
    {
        var result = await _projectService.GetProjectListAsync();

        if (!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }

        return Ok(result);

    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetProject(int id)
    {
        var result = await _projectService.GetProjectAsync(id);

        if (!result.IsSuccess)
        {
            return NotFound(result);
        }

        return Ok(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProject(
        int id,
        [FromBody] ProjectFormRequestDto request)
    {
        if (id <= 0 || string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest("The project ID and name are required.");
        }

        var result = await _projectService.UpdateProjectAsync(id, request);

        if (!result.IsSuccess)
        {
            return NotFound(result);
        }

        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProject(int id)
    {
        if (id <= 0)
        {
            return BadRequest("The project ID is invalid.");
        }

        var result = await _projectService.DeleteProjectAsync(id);

        if (!result.IsSuccess)
        {
            return NotFound(result);
        }

        return Ok(result);
    }
}
