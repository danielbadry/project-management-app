namespace AppHost.Web.Components.Pages.Stories;

using AppHost.Web.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Shared.Dtos.Story;

public partial class Index
{
    private bool _isLoading = true;
    private List<StoryRecordDto> _stories = [];
    private string _errorMessage = string.Empty;
    private string _actionError = string.Empty;
    private int? _deletingId;

    [Parameter] public int ProjectId { get; set; }
    [Inject] private StoriesService StoriesService { get; set; } = default!;
    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

    protected override async Task OnParametersSetAsync() => await LoadStoriesAsync();

    private async Task LoadStoriesAsync()
    {
        _isLoading = true;
        _errorMessage = string.Empty;
        var result = await StoriesService.GetStoriesAsync(ProjectId);
        _stories = result.Succeeded && result.Data is not null ? result.Data : [];
        _errorMessage = result.Succeeded ? string.Empty : result.ErrorMessage ?? "Stories could not be loaded.";
        _isLoading = false;
    }

    private async Task DeleteStoryAsync(StoryRecordDto story)
    {
        if (!await JSRuntime.InvokeAsync<bool>("confirm", $"Delete '{story.Title}'?"))
        {
            return;
        }

        _deletingId = story.Id;
        _actionError = string.Empty;
        try
        {
            var result = await StoriesService.DeleteStoryAsync(ProjectId, story.Id);
            if (result.Succeeded)
            {
                _stories.RemoveAll(item => item.Id == story.Id);
            }
            else
            {
                _actionError = result.ErrorMessage ?? "The story could not be deleted.";
            }
        }
        finally
        {
            _deletingId = null;
        }
    }
}
