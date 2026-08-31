namespace AppHost.Web.Components.Pages.SubTasks;

using AppHost.Web.Services;
using Microsoft.AspNetCore.Components;
using Shared.Dtos.SubTask;

public partial class Edit
{
    private bool _isLoading = true;
    private SubTaskRecordDto? _subTask;
    private string _errorMessage = string.Empty;

    [Parameter] public int StoryId { get; set; }
    [Parameter] public int ProjectId { get; set; }
    [Parameter] public int Id { get; set; }
    [Inject] private SubTasksService SubTasksService { get; set; } = default!;

    protected override async Task OnParametersSetAsync()
    {
        _isLoading = true;
        var result = await SubTasksService.GetSubTaskAsync(StoryId, Id);
        _subTask = result.Succeeded ? result.Data : null;
        _errorMessage = result.Succeeded ? string.Empty : result.ErrorMessage ?? "SubTask not found.";
        _isLoading = false;
    }
}
