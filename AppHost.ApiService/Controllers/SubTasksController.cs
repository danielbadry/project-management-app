namespace AppHost.ApiService.Controllers;

using AppHost.ApiService.Services.ProjectManagement.SubTask;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

}