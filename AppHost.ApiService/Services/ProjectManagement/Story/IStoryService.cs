using Shared.Dtos;
using Shared.Dtos.Story;

namespace AppHost.ApiService.Services.ProjectManagement;

public interface IStoryService
{
    Task<ApiResponse<StoryRecordDto>> CreateStoryAsync(int projectId, int ownerId, StoryFromRequestDto request);
    Task<ApiResponse<List<StoryRecordDto>>> GetStoriesAsync(int projectId);
    Task<ApiResponse<StoryRecordDto>> GetStoryAsync(int projectId, int id);
    Task<ApiResponse<StoryRecordDto>> UpdateStoryAsync(int projectId, int id, StoryFromRequestDto request);
    Task<ApiResponse<bool>> DeleteStoryAsync(int projectId, int id);
}
