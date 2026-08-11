namespace AppHost.Web.Services;

using Shared.Dtos.Story;

public class StoriesService(ApiClient apiClient)
{
    private static string StoriesUrl(int projectId) => $"api/projects/{projectId}/stories";

    public async Task<ServiceResult<StoryRecordDto>> CreateStoryAsync(
        StoryFromRequestDto request)
    {
        return await apiClient.PostAsync<StoryRecordDto>(
            StoriesUrl(request.ProjectId), request, "create story", "Creating the story failed.");
    }

    public Task<ServiceResult<List<StoryRecordDto>>> GetStoriesAsync(int projectId) =>
        apiClient.GetAsync<List<StoryRecordDto>>(
            StoriesUrl(projectId), "load stories", "Loading stories failed.");

    public Task<ServiceResult<StoryRecordDto>> GetStoryAsync(int projectId, int id) =>
        apiClient.GetAsync<StoryRecordDto>(
            $"{StoriesUrl(projectId)}/{id}", "load story", "Loading the story failed.");

    public async Task<ServiceResult<StoryRecordDto>> UpdateStoryAsync(
        int projectId,
        int id,
        StoryFromRequestDto request)
    {
        return await apiClient.PutAsync<StoryRecordDto>(
            $"{StoriesUrl(projectId)}/{id}", request, "update story", "Updating the story failed.");
    }

    public async Task<ServiceResult> DeleteStoryAsync(int projectId, int id)
    {
        var result = await apiClient.DeleteAsync<bool>(
            $"{StoriesUrl(projectId)}/{id}", "delete story", "Deleting the story failed.");

        return result.Succeeded && result.Data
            ? ServiceResult.Success()
            : ServiceResult.Failure(result.ErrorMessage ?? "Deleting the story failed.");
    }
}
