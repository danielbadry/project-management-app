using AppHost.ApiService.Data;
using AppHost.ApiService.Services.ProjectManagement.SubTask;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos;
using Shared.Dtos.SubTask;

namespace AppHost.ApiService.Services.ProjectManagement.SubTask;

public class SubTaskService : ISubTaskService
{
    private readonly AppDbContext _dbContext;
    public SubTaskService(
        AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ApiResponse<List<SubTaskRecordDto>>> GetSubTasksAsync(int storyId)
    {
        if (storyId <= 0)
        {
            return ApiResponse<List<SubTaskRecordDto>>.Fail("loading subTasks failed");
        }

        var subTasks = await _dbContext.SubTasks
        .AsNoTracking()
        .Where(subTask => subTask.StoryId == storyId)
        .OrderByDescending(subTask => subTask.CreatedAtUtc)
        .Select(subTask => new SubTaskRecordDto
        {
            Id = subTask.Id,
            Description = subTask.Description,
            IsActive = subTask.IsActive,
            Title = subTask.Title,
            StoryId = subTask.StoryId
        })
        .ToListAsync();

        return ApiResponse<List<SubTaskRecordDto>>.Success(subTasks, "subTasks loaded successfully");
    }

}