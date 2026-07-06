using Shared.Dtos.ProjectForm;
using AppHost.Web.Services;
using Microsoft.AspNetCore.Components;

namespace AppHost.Web.Components.Pages.Projects.Components;

public partial class ProjectForm
{
    private ProjectFormRequestDto _projectFormRequestDto = new();
    private string _projectFormErrorMessage = string.Empty;

    [Inject]
    private ProjectsService ProjectsService { get; set; } = default!;

    [Inject]
    private NavigationManager Navigation { get; set; } = null!;

    [Parameter]
    public bool IsEditMode { get; set; } = false;

    [Parameter]
    public ProjectFormResponseDto? ProjectInfo { get; set; }

    protected override void OnParametersSet()
    {
        if (ProjectInfo is not null)
        {
            _projectFormRequestDto = new ProjectFormRequestDto { Name = ProjectInfo.Name, Description = ProjectInfo.Description, IsActive = ProjectInfo.IsActive };
        }
        base.OnParametersSet();
    }

    private Task HandleSave()
    {
        return IsEditMode ? HandleUpdate() : HandleCreate();
    }

    public async Task HandleCreate()
    {
        _projectFormErrorMessage = "";

        var result = await ProjectsService.CreateNewProject(_projectFormRequestDto);
        if (result.Succeeded && result.Data is not null)
        {
            Navigation.NavigateTo($"/projects/{result.Data.Id}");
            return;
        }
        else
        {
            _projectFormErrorMessage = result.ErrorMessage ?? "";
        }
    }


    public async Task HandleUpdate()
    {
        _projectFormErrorMessage = "";

        if (ProjectInfo is null)
        {
            _projectFormErrorMessage = "The project could not be loaded for editing.";
            return;
        }

        var result = await ProjectsService.UpdateProject(ProjectInfo.Id, _projectFormRequestDto);
        if (result.Succeeded && result.Data is not null)
        {
            Navigation.NavigateTo($"/projects/{result.Data.Id}");
            return;
        }
        else
        {
            _projectFormErrorMessage = result.ErrorMessage ?? "";
        }
    }
}
