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

    public async Task<ApiResponse<SubTaskRecordDto>> CreateSubTasksAsync(SubTaskFormDto request)
    {
        if (request is null || request.StoryId <= 0 || string.IsNullOrWhiteSpace(request.Title) || string.IsNullOrWhiteSpace(request.Description))
        {
            return ApiResponse<SubTaskRecordDto>.Fail("A story and subtask title are required.");
        }

        if (!await _dbContext.Stories.AnyAsync(story => story.Id == request.StoryId))
        {
            return ApiResponse<SubTaskRecordDto>.Fail("Story not found.");
        }

        var subTask = new Entities.ProjectManagement.SubTask
        {
            Title = request.Title.Trim(),
            Description = request.Description?.Trim() ?? string.Empty,
            StoryId = request.StoryId,
            IsActive = true
        };

        _dbContext.SubTasks.Add(subTask);
        await _dbContext.SaveChangesAsync();

        return ApiResponse<SubTaskRecordDto>.Success(Map(subTask), "Subtask created successfully");
    }

    private static SubTaskRecordDto Map(Entities.ProjectManagement.SubTask subTask) => new()
    {
        Id = subTask.Id,
        Title = subTask.Title,
        Description = subTask.Description,
        StoryId = subTask.StoryId,
        IsActive = subTask.IsActive
    };
}
