using Shared.Dtos.ProjectForm;
using AppHost.Web.Services;
using Microsoft.AspNetCore.Components;

namespace AppHost.Web.Components.Pages.Projects;

public partial class View
{
    private bool _isLoading = true;
    private ProjectFormResponseDto? _projectInfo;
    private string _errorMessage = string.Empty;

    [Parameter]
    public int Id { get; set; }

    [Inject]
    private ProjectsService ProjectsService { get; set; } = default!;

    protected override async Task OnInitializedAsync() => await LoadProjectInfoAsync();

    private async Task LoadProjectInfoAsync()
    {
        _isLoading = true;
        _errorMessage = string.Empty;

        var result = await ProjectsService.GetProjectAsync(Id);

        if (result.Succeeded && result.Data is not null)
        {
            _projectInfo = result.Data;
        }
        else
        {
            _projectInfo = null;
            _errorMessage = result.ErrorMessage ?? "The project could not be loaded.";
        }

        _isLoading = false;
    }
}
