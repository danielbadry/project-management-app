using Shared.Dtos;
using Shared.Dtos.SubTask;

namespace AppHost.ApiService.Services.ProjectManagement.SubTask;

public interface ISubTaskService
{
    Task<ApiResponse<List<SubTaskRecordDto>>> GetSubTasksAsync(int storyId);
}
