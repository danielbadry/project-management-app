namespace AppHost.Web.Components.Pages.SubTasks;

using AppHost.Web.Services;
using Microsoft.AspNetCore.Components;
using Shared.Dtos.SubTask;
public partial class Index
{
    private bool _isLoading = false;
    [Parameter]
    public int StoryId { set; get; }

    [Parameter]
    public int ProjectId { set; get; }

    private List<SubTaskRecordDto> _subTasks = [];
    private string _errorMessage = default!;

    [Inject]
    private SubTasksService SubTasksService { set; get; } = default!;

    protected override async Task OnParametersSetAsync() => await LoadSubTasksAsync();

    private async Task LoadSubTasksAsync()
    {
        _isLoading = true;

        var result = await SubTasksService.GetSubTasksAsync(StoryId);
        _subTasks = result.Succeeded && result.Data is not null ? result.Data : [];
        _errorMessage = result.ErrorMessage is not null ? result.ErrorMessage : "";
        _isLoading = false;
    }
}