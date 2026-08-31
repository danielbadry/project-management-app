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
    private int? _loadedSubTaskId;

    [Inject] private StoriesService StoriesService { get; set; } = default!;
    [Inject] private SubTasksService SubTasksService { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;

    [Parameter] public SubTaskRecordDto SubTaskInfo { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        _isLoadingOptions = true;
        _request.StoryId = StoryId;

        var result = await StoriesService.GetStoriesAsync(ProjectId);
        _stories = result.Succeeded && result.Data is not null ? result.Data : [];

        _isLoadingOptions = false;
    }

    protected override void OnParametersSet()
    {
        if (SubTaskInfo is not null && _loadedSubTaskId != SubTaskInfo.Id)
        {
            _request = new SubTaskFormDto
            {
                Title = SubTaskInfo.Title,
                Description = SubTaskInfo.Description,
                StoryId = SubTaskInfo.StoryId,
            };
            _loadedSubTaskId = SubTaskInfo.Id;
        }
    }

    private async Task HandleSaveAsync()
    {
        _isSaving = true;
        _errorMessage = string.Empty;

        try
        {
            var result = IsEditMode && SubTaskInfo is not null
                ? await SubTasksService.UpdateSubTaskAsync(SubTaskInfo.StoryId, SubTaskInfo.Id, _request)
                : await SubTasksService.CreateSubTasksAsync(_request);
            if (result.Succeeded && result.Data is not null)
            {
                Navigation.NavigateTo($"/projects/{ProjectId}/stories/{result.Data.StoryId}/subtasks");
                return;
            }

            _errorMessage = result.ErrorMessage ?? "The subtask could not be saved.";
        }
        finally
        {
            _isSaving = false;
        }
    }

}
