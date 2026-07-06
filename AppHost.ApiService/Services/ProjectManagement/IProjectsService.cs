using Shared.Dtos;
using Shared.Dtos.ProjectForm;

namespace AppHost.ApiService.Services.ProjectManagement;

public interface IProjectsService
{
    Task<ApiResponse<ProjectFormResponseDto>> CreateProjectAsync(
        ProjectFormRequestDto request);


    Task<ApiResponse<List<ProjectFormResponseDto>>> GetProjectListAsync();

    Task<ApiResponse<ProjectFormResponseDto>> GetProjectAsync(int id);
}
