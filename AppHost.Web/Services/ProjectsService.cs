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
}
