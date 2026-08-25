using AppHost.Web.Services;
using Microsoft.AspNetCore.Components;
using Shared.Dtos.SubTask;
using Shared.Dtos.Story;

namespace AppHost.Web.Components.Pages.SubTasks.Components;

partial class SubtaskForm
{
    [Parameter, EditorRequired]
    public int ProjectId { set; get; }

    [Parameter]
    public int StoryId { set; get; }

    [Parameter]
    public bool IsEditMode { set; get; } = false;

    private List<StoryRecordDto> _stories = [];
    private SubTaskFormDto _request = new();

    private bool _isLoadingOptions = false;
    private bool _isSaving = false;
    private string _errorMessage = string.Empty;

    [Inject] private StoriesService StoriesService { get; set; } = default!;
    [Inject] private SubTasksService SubTasksService { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        _isLoadingOptions = true;
        _request.StoryId = StoryId;

        var result = await StoriesService.GetStoriesAsync(ProjectId);
        _stories = result.Succeeded && result.Data is not null ? result.Data : [];

        _isLoadingOptions = false;
    }

    private async Task HandleSaveAsync()
    {
        _isSaving = true;
        _errorMessage = string.Empty;

        try
        {
            var result = await SubTasksService.CreateSubTasksAsync(_request);
            if (result.Succeeded)
            {
                Navigation.NavigateTo($"/projects/{ProjectId}/stories/{StoryId}/subtasks");
                return;
            }
        }
        finally
        {
            _isSaving = false;
        }
    }

}
