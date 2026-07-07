using AppHost.ApiService.Data;
using AppHost.ApiService.Entities.ProjectManagement;
using Shared.Dtos;
using Shared.Dtos.Story;
using Microsoft.EntityFrameworkCore;

namespace AppHost.ApiService.Services.ProjectManagement.Story;

public class StoryService : IStoryService
{
    private readonly AppDbContext _dbContext;
    public StoryService(
        AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<> CreateStoryAsync(StoryFromRequestDto request)
    {

    }

}
