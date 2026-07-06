namespace AppHost.Web.Services;

using Shared.Dtos.ProjectForm;

public class ProjectsService(ApiClient apiClient)
{
    private readonly string projectsUrl = "api/projects";
    private readonly ApiClient _apiClient = apiClient;

    public async Task<ServiceResult<ProjectFormResponseDto>> HandleSave(
        ProjectFormRequestDto projectsFormData)
    {
        var result = await _apiClient.PostAsync<ProjectFormResponseDto>(
           projectsUrl,
           projectsFormData,
           "projects",
           "Saving Project data failed. Please check your details.");


        if (!result.Succeeded || result.Data is null)
        {
            return ServiceResult<ProjectFormResponseDto>.Failure(
                result.ErrorMessage ?? "Saving failed. Please try again.");
        }

        return ServiceResult<ProjectFormResponseDto>.Success(result.Data);
    }

    public async Task<ServiceResult<List<ProjectFormResponseDto>>> GetProjectListAsync()
    {
        var result = await _apiClient.GetAsync<List<ProjectFormResponseDto>>(
            projectsUrl,
            "projects",
            "Loading projects failed. Please try again.");


        if (!result.Succeeded || result.Data is null)
        {
            return ServiceResult<List<ProjectFormResponseDto>>.Failure(
                result.ErrorMessage ?? "Loading projects failed. Please try again.");
        }

        return ServiceResult<List<ProjectFormResponseDto>>.Success(result.Data);

    }

    public async Task<ServiceResult<ProjectFormResponseDto>> GetProjectAsync(int id)
    {
        if (id <= 0)
        {
            return ServiceResult<ProjectFormResponseDto>.Failure(
                "The project ID is invalid.");
        }

        return await _apiClient.GetAsync<ProjectFormResponseDto>(
            $"{projectsUrl}/{id}",
            "get project",
            "Loading the project failed. Please try again.");
    }
}
