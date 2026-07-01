using Shared.Dtos.ProjectForm;

namespace AppHost.Web.Components.Pages.Projects;

public partial class ProjectForm()
{
    public ProjectFormRequestDto ProjectFormRequestDto { set; get; } = new();

    public string ProjectFormErrorMessage { set; get; } = string.Empty;

    public async Task<bool> HandleSave()
    {
        return true;
    }

}