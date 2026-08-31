using Shared.Dtos.SubTask;

namespace AppHost.Web.Services;

public class SubTasksService(ApiClient apiClient)
{
    private static string SubTasksUrl(int storyId) => $"api/stories/{storyId}/subtasks";

    public Task<ServiceResult<List<SubTaskRecordDto>>> GetSubTasksAsync(int storyId) =>
        apiClient.GetAsync<List<SubTaskRecordDto>>(
            SubTasksUrl(storyId), "load sub tasks", "Loading subtasks failed.");
    public Task<ServiceResult<SubTaskRecordDto>> GetSubTaskAsync(int storyId, int subTaskId) =>
        apiClient.GetAsync<SubTaskRecordDto>(
            $"{SubTasksUrl(storyId)}/{subTaskId}", "load sub task", "Loading subtask failed.");

    public Task<ServiceResult<SubTaskRecordDto>> CreateSubTasksAsync(SubTaskFormDto request) =>
        apiClient.PostAsync<SubTaskRecordDto>(
            SubTasksUrl(request.StoryId), request, "create sub tasks", "Create subtask failed.");

    public Task<ServiceResult<SubTaskRecordDto>> UpdateSubTaskAsync(
        int storyId,
        int subTaskId,
        SubTaskFormDto request) =>
        apiClient.PutAsync<SubTaskRecordDto>(
            $"{SubTasksUrl(storyId)}/{subTaskId}", request, "update subtask", "Update subtask failed.");
}
