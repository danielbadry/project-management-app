namespace AppHost.Web.Components.Pages.Stories;

using AppHost.Web.Services;
using Microsoft.AspNetCore.Components;
using Shared.Dtos.Story;

public partial class View
{
    private bool _isLoading = true;
    private StoryRecordDto? _story;
    private string _errorMessage = string.Empty;

    [Parameter] public int ProjectId { get; set; }
    [Parameter] public int Id { get; set; }
    [Inject] private StoriesService StoriesService { get; set; } = default!;

    protected override async Task OnParametersSetAsync()
    {
        _isLoading = true;
        var result = await StoriesService.GetStoryAsync(ProjectId, Id);
        _story = result.Succeeded ? result.Data : null;
        _errorMessage = result.Succeeded ? string.Empty : result.ErrorMessage ?? "Story not found.";
        _isLoading = false;
    }
}
