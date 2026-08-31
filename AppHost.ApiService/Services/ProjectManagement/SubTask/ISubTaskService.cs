using Shared.Dtos;
using Shared.Dtos.SubTask;

namespace AppHost.ApiService.Services.ProjectManagement.SubTask;

public interface ISubTaskService
{
    Task<ApiResponse<List<SubTaskRecordDto>>> GetSubTasksAsync(int storyId);
    Task<ApiResponse<SubTaskRecordDto>> GetSubTaskAsync(int storyId, int id);
    Task<ApiResponse<SubTaskRecordDto>> CreateSubTasksAsync(SubTaskFormDto request);
    Task<ApiResponse<SubTaskRecordDto>> UpdateSubTaskAsync(int storyId, int id, SubTaskFormDto request);
}
