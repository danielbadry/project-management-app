using Shared.Dtos.ProjectForm;
using AppHost.Web.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AppHost.Web.Components.Pages.Projects;

public partial class Index
{
    private bool _isLoading = true;
    private List<ProjectFormResponseDto> _projectList = [];
    private string _errorMessage = string.Empty;
    private string _actionErrorMessage = string.Empty;
    private int? _deletingProjectId;

    [Inject]
    private ProjectsService ProjectsService { get; set; } = default!;

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    protected override async Task OnInitializedAsync() => await LoadProjectsAsync();

    private async Task LoadProjectsAsync()
    {
        _errorMessage = string.Empty;
        _isLoading = true;
        var projectResponse = await ProjectsService.GetProjectListAsync();

        if (projectResponse.Succeeded && projectResponse.Data is not null)
            _projectList = projectResponse.Data;
        else
            _errorMessage = projectResponse.ErrorMessage ?? "Something went wrong while loading projects.";

        _isLoading = false;
    }

    private async Task DeleteProjectAsync(ProjectFormResponseDto project)
    {
        var confirmed = await JSRuntime.InvokeAsync<bool>(
            "confirm",
            $"Delete '{project.Name}'? This action cannot be undone.");

        if (!confirmed)
        {
            return;
        }

        _actionErrorMessage = string.Empty;
        _deletingProjectId = project.Id;

        try
        {
            var result = await ProjectsService.DeleteProjectAsync(project.Id);
            if (result.Succeeded)
            {
                _projectList.RemoveAll(item => item.Id == project.Id);
            }
            else
            {
                _actionErrorMessage = result.ErrorMessage ??
                    "The project could not be deleted.";
            }
        }
        finally
        {
            _deletingProjectId = null;
        }
    }
}
