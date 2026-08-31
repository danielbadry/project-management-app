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


    public async Task<ApiResponse<SubTaskRecordDto>> GetSubTaskAsync(int storyId, int id)
    {
        if (storyId <= 0 || id <= 0)
        {
            return ApiResponse<SubTaskRecordDto>.Fail("A valid story and subtask are required.");
        }

        var subTask = await _dbContext.SubTasks
        .AsNoTracking()
        .Where(subTask => subTask.StoryId == storyId && subTask.Id == id)
        .Select(subTask => new SubTaskRecordDto
        {
            Id = subTask.Id,
            Description = subTask.Description,
            IsActive = subTask.IsActive,
            Title = subTask.Title,
            StoryId = subTask.StoryId
        }).FirstOrDefaultAsync();

        return subTask is null
            ? ApiResponse<SubTaskRecordDto>.Fail("Subtask not found.")
            : ApiResponse<SubTaskRecordDto>.Success(subTask, "Subtask loaded successfully");
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

    public async Task<ApiResponse<SubTaskRecordDto>> UpdateSubTaskAsync(
        int storyId,
        int id,
        SubTaskFormDto request)
    {
        if (request is null || request.StoryId <= 0 || string.IsNullOrWhiteSpace(request.Title) || string.IsNullOrWhiteSpace(request.Description))
        {
            return ApiResponse<SubTaskRecordDto>.Fail("A story, subtask title, and description are required.");
        }

        if (!await _dbContext.Stories.AnyAsync(story => story.Id == request.StoryId))
        {
            return ApiResponse<SubTaskRecordDto>.Fail("Story not found.");
        }

        var subTask = await _dbContext.SubTasks
            .FirstOrDefaultAsync(item => item.StoryId == storyId && item.Id == id);
        if (subTask is null)
        {
            return ApiResponse<SubTaskRecordDto>.Fail("Subtask not found.");
        }

        subTask.Title = request.Title.Trim();
        subTask.Description = request.Description.Trim();
        subTask.StoryId = request.StoryId;
        subTask.UpdatedAtUtc = DateTimeOffset.UtcNow;
        await _dbContext.SaveChangesAsync();

        return ApiResponse<SubTaskRecordDto>.Success(Map(subTask), "Subtask updated successfully");
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
