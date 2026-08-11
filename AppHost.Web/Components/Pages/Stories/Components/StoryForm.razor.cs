namespace AppHost.Web.Components.Pages.Stories.Components;

using AppHost.Web.Services;
using Microsoft.AspNetCore.Components;
using Shared.Dtos.ProjectForm;
using Shared.Dtos.Story;
using Shared.Dtos.User;

public partial class StoryForm
{
    private StoryFromRequestDto _request = new();
    private List<ProjectFormResponseDto> _projects = [];
    private List<UserOptionDto> _users = [];
    private string _errorMessage = string.Empty;
    private bool _isSaving;
    private bool _isLoadingOptions = true;
    private int? _loadedStoryId;

    [Parameter]
    public int? InitialProjectId { get; set; }

    [Parameter]
    public bool IsEditMode { get; set; }

    [Parameter]
    public StoryRecordDto? StoryInfo { get; set; }

    [Inject] private StoriesService StoriesService { get; set; } = default!;
    [Inject] private ProjectsService ProjectsService { get; set; } = default!;
    [Inject] private UsersService UsersService { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;

    private string CancelUrl => _request.ProjectId > 0
        ? $"/projects/{_request.ProjectId}/stories"
        : "/projects";

    protected override async Task OnInitializedAsync()
    {
        var projectsTask = ProjectsService.GetProjectListAsync();
        var usersTask = UsersService.GetUsersAsync();
        await Task.WhenAll(projectsTask, usersTask);

        var projectsResult = await projectsTask;
        var usersResult = await usersTask;

        if (projectsResult.Succeeded && projectsResult.Data is not null)
        {
            _projects = projectsResult.Data;
        }
        else
        {
            _errorMessage = projectsResult.ErrorMessage ?? "Projects could not be loaded.";
        }

        if (usersResult.Succeeded && usersResult.Data is not null)
        {
            _users = usersResult.Data;
        }
        else if (string.IsNullOrWhiteSpace(_errorMessage))
        {
            _errorMessage = usersResult.ErrorMessage ?? "Users could not be loaded.";
        }

        _isLoadingOptions = false;
    }

    protected override void OnParametersSet()
    {
        if (StoryInfo is not null && _loadedStoryId != StoryInfo.Id)
        {
            _request = new StoryFromRequestDto
            {
                Title = StoryInfo.Title,
                Description = StoryInfo.Description,
                ProjectId = StoryInfo.ProjectId,
                AssignedId = StoryInfo.AssignedId,
                IsActive = StoryInfo.IsActive
            };
            _loadedStoryId = StoryInfo.Id;
        }
        else if (!IsEditMode && InitialProjectId is > 0 && _request.ProjectId == 0)
        {
            _request.ProjectId = InitialProjectId.Value;
        }
    }

    private async Task HandleSaveAsync()
    {
        _errorMessage = string.Empty;
        _isSaving = true;

        try
        {
            var result = IsEditMode && StoryInfo is not null
                ? await StoriesService.UpdateStoryAsync(StoryInfo.ProjectId, StoryInfo.Id, _request)
                : await StoriesService.CreateStoryAsync(_request);

            if (result.Succeeded && result.Data is not null)
            {
                Navigation.NavigateTo($"/projects/{result.Data.ProjectId}/stories/{result.Data.Id}");
                return;
            }

            _errorMessage = result.ErrorMessage ?? "The story could not be saved.";
        }
        finally
        {
            _isSaving = false;
        }
    }
}
