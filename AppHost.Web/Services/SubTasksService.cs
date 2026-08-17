using Shared.Dtos.SubTask;

namespace AppHost.Web.Services;

public class SubTasksService(ApiClient apiClient)
{
    private static string SubTasksUrl(int storyId) => $"api/stories/{storyId}/subtasks";

    public Task<ServiceResult<List<SubTaskRecordDto>>> GetSubTasksAsync(int storyId) =>
        apiClient.GetAsync<List<SubTaskRecordDto>>(
            SubTasksUrl(storyId), "load sub tasks", "Loading subtasks failed.");
}