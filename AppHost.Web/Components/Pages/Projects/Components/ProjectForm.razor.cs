using Shared.Dtos.ProjectForm;
using AppHost.Web.Services;
using Microsoft.AspNetCore.Components;

namespace AppHost.Web.Components.Pages.Projects.Components;

public partial class ProjectForm()
{
    private readonly ProjectFormRequestDto _projectFormRequestDto = new();
    private string _projectFormErrorMessage = string.Empty;

    [Inject]
    private ProjectsService ProjectsService { set; get; } = default!;

    [Inject]
    private NavigationManager Navigation { get; set; } = null!;

    public async Task HandleSave()
    {
        _projectFormErrorMessage = "";

        var result = await ProjectsService.HandleSave(_projectFormRequestDto);
        if (result.Succeeded && result.Data is not null)
        {
            Navigation.NavigateTo($"/project/{result.Data.Id}");
            return;
        }
        else
        {
            _projectFormErrorMessage = result.ErrorMessage ?? "";
        }
    }
}