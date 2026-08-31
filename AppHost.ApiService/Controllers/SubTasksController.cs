namespace AppHost.ApiService.Controllers;

using AppHost.ApiService.Services.ProjectManagement.SubTask;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos.SubTask;

[ApiController]
[Authorize]
[Route("api/stories/{storyId:int}/subtasks")]
public class SubTasksController(ISubTaskService subTaskService) : ControllerBase
{

    [HttpGet]
    public async Task<IActionResult> GetSubTasks(int storyId)
    {
        var result = await subTaskService.GetSubTasksAsync(storyId);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateSubTasks([FromBody] SubTaskFormDto request)
    {
        var result = await subTaskService.CreateSubTasksAsync(request);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }


    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetSubTask(int storyId, int id)
    {
        var result = await subTaskService.GetSubTaskAsync(storyId, id);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateSubTask(
        int storyId,
        int id,
        [FromBody] SubTaskFormDto request)
    {
        var result = await subTaskService.UpdateSubTaskAsync(storyId, id, request);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

}
