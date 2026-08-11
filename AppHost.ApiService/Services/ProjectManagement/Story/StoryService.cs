using AppHost.ApiService.Data;
using AppHost.ApiService.Entities.ProjectManagement;
using Shared.Dtos;
using Shared.Dtos.Story;
using Microsoft.EntityFrameworkCore;

namespace AppHost.ApiService.Services.ProjectManagement.Story;

public class StoryService : IStoryService
{
    private readonly AppDbContext _dbContext;
    public StoryService(
        AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ApiResponse<StoryRecordDto>> CreateStoryAsync(
        int projectId,
        int ownerId,
        StoryFromRequestDto request)
    {
        if (projectId <= 0 || ownerId <= 0 || request is null ||
            string.IsNullOrWhiteSpace(request.Title))
        {
            return ApiResponse<StoryRecordDto>.Fail("A project and story title are required.");
        }

        if (request.ProjectId != projectId)
        {
            return ApiResponse<StoryRecordDto>.Fail("The selected project is invalid.");
        }

        if (!await _dbContext.Projects.AnyAsync(project => project.Id == projectId))
        {
            return ApiResponse<StoryRecordDto>.Fail("Project not found.");
        }

        if (request.AssignedId is int assignedId &&
            !await _dbContext.Users.AnyAsync(user => user.Id == assignedId))
        {
            return ApiResponse<StoryRecordDto>.Fail("Assigned user not found.");
        }

        var story = new Entities.ProjectManagement.Story
        {
            Title = request.Title.Trim(),
            Description = request.Description?.Trim() ?? string.Empty,
            ProjectId = projectId,
            OwnerId = ownerId,
            AssignedId = request.AssignedId,
            IsActive = request.IsActive
        };

        _dbContext.Stories.Add(story);
        await _dbContext.SaveChangesAsync();

        return ApiResponse<StoryRecordDto>.Success(Map(story), "Story created successfully");
    }

    public async Task<ApiResponse<List<StoryRecordDto>>> GetStoriesAsync(int projectId)
    {
        if (projectId <= 0)
        {
            return ApiResponse<List<StoryRecordDto>>.Fail("The project ID is invalid.");
        }

        var stories = await _dbContext.Stories
            .AsNoTracking()
            .Where(story => story.ProjectId == projectId)
            .OrderByDescending(story => story.CreatedAtUtc)
            .Select(story => new StoryRecordDto
            {
                Id = story.Id,
                Title = story.Title,
                Description = story.Description,
                ProjectId = story.ProjectId,
                AssignedId = story.AssignedId,
                IsActive = story.IsActive
            })
            .ToListAsync();

        return ApiResponse<List<StoryRecordDto>>.Success(stories, "Stories loaded successfully");
    }

    public async Task<ApiResponse<StoryRecordDto>> GetStoryAsync(int projectId, int id)
    {
        var story = await FindStoryAsync(projectId, id, asTracking: false);
        return story is null
            ? ApiResponse<StoryRecordDto>.Fail("Story not found.")
            : ApiResponse<StoryRecordDto>.Success(Map(story), "Story loaded successfully");
    }

    public async Task<ApiResponse<StoryRecordDto>> UpdateStoryAsync(
        int projectId,
        int id,
        StoryFromRequestDto request)
    {
        if (request is null || string.IsNullOrWhiteSpace(request.Title))
        {
            return ApiResponse<StoryRecordDto>.Fail("A story title is required.");
        }

        if (!await _dbContext.Projects.AnyAsync(project => project.Id == request.ProjectId))
        {
            return ApiResponse<StoryRecordDto>.Fail("Project not found.");
        }

        var story = await FindStoryAsync(projectId, id, asTracking: true);
        if (story is null)
        {
            return ApiResponse<StoryRecordDto>.Fail("Story not found.");
        }

        if (request.AssignedId is int assignedId &&
            !await _dbContext.Users.AnyAsync(user => user.Id == assignedId))
        {
            return ApiResponse<StoryRecordDto>.Fail("Assigned user not found.");
        }

        story.Title = request.Title.Trim();
        story.Description = request.Description?.Trim() ?? string.Empty;
        story.ProjectId = request.ProjectId;
        story.AssignedId = request.AssignedId;
        story.IsActive = request.IsActive;
        story.UpdatedAtUtc = DateTimeOffset.UtcNow;
        await _dbContext.SaveChangesAsync();

        return ApiResponse<StoryRecordDto>.Success(Map(story), "Story updated successfully");
    }

    public async Task<ApiResponse<bool>> DeleteStoryAsync(int projectId, int id)
    {
        var story = await FindStoryAsync(projectId, id, asTracking: true);
        if (story is null)
        {
            return ApiResponse<bool>.Fail("Story not found.");
        }

        _dbContext.Stories.Remove(story);
        await _dbContext.SaveChangesAsync();
        return ApiResponse<bool>.Success(true, "Story deleted successfully");
    }

    private Task<Entities.ProjectManagement.Story?> FindStoryAsync(
        int projectId,
        int id,
        bool asTracking)
    {
        var query = _dbContext.Stories
            .Where(story => story.ProjectId == projectId && story.Id == id);

        if (!asTracking)
        {
            query = query.AsNoTracking();
        }

        return query.FirstOrDefaultAsync();
    }

    private static StoryRecordDto Map(Entities.ProjectManagement.Story story) => new()
    {
        Id = story.Id,
        Title = story.Title,
        Description = story.Description,
        ProjectId = story.ProjectId,
        AssignedId = story.AssignedId,
        IsActive = story.IsActive
    };
}
