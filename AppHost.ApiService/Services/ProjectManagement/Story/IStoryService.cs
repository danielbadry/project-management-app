using Shared.Dtos;
using Shared.Dtos.ProjectForm;
using Shared.Dtos.Story;

namespace AppHost.ApiService.Services.ProjectManagement;

public interface IStoryService
{
    Task<ApiResponse<StoryRecordDto>> CreateStoryAsync(
        StoryFromRequestDto request);
}
