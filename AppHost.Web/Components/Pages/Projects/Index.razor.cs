using Shared.Dtos.ProjectForm;
using AppHost.Web.Services;
using Microsoft.AspNetCore.Components;

namespace AppHost.Web.Components.Pages.Projects;

public partial class Index
{
    private bool _isLoading = true;
    private List<ProjectFormResponseDto> _projectList = [];
    private string _errorMessage = string.Empty;

    [Inject]
    private ProjectsService ProjectsService { get; set; } = default!;

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
}
