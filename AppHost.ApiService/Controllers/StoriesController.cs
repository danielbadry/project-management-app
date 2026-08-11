namespace AppHost.ApiService.Controllers;

using System.Security.Claims;
using AppHost.ApiService.Services.ProjectManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos.Story;

[ApiController]
[Authorize]
[Route("api/projects/{projectId:int}/stories")]
public class StoriesController(IStoryService storyService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateStory(
        int projectId,
        [FromBody] StoryFromRequestDto request)
    {
        if (!TryGetUserId(out var userId))
        {
            return Unauthorized();
        }

        var result = await storyService.CreateStoryAsync(projectId, userId, request);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetStories(int projectId)
    {
        var result = await storyService.GetStoriesAsync(projectId);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetStory(int projectId, int id)
    {
        var result = await storyService.GetStoryAsync(projectId, id);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateStory(
        int projectId,
        int id,
        [FromBody] StoryFromRequestDto request)
    {
        var result = await storyService.UpdateStoryAsync(projectId, id, request);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteStory(int projectId, int id)
    {
        var result = await storyService.DeleteStoryAsync(projectId, id);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    private bool TryGetUserId(out int userId)
    {
        var value = User.FindFirstValue(ClaimTypes.NameIdentifier) ??
            User.FindFirstValue("sub");
        return int.TryParse(value, out userId) && userId > 0;
    }
}
