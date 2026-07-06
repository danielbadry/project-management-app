using AppHost.ApiService.Data;
using AppHost.ApiService.Entities.ProjectManagement;
using Shared.Dtos;
using Shared.Dtos.ProjectForm;
using Microsoft.EntityFrameworkCore;

namespace AppHost.ApiService.Services.ProjectManagement;

public class ProjectsService : IProjectsService
{
    private readonly AppDbContext _dbContext;
    public ProjectsService(
        AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ApiResponse<ProjectFormResponseDto>> CreateProjectAsync(ProjectFormRequestDto request)
    {
        if (request is null || string.IsNullOrWhiteSpace(request.Name))
        {
            return ApiResponse<ProjectFormResponseDto>.Fail("Form Data Is Empty");
        }

        var project = new Projects
        {
            Name = request.Name,
            Description = request.Description,
            IsActive = request.IsActive
        };

        _dbContext.Projects.Add(project);
        await _dbContext.SaveChangesAsync();


        return ApiResponse<ProjectFormResponseDto>.Success(new ProjectFormResponseDto { Id = project.Id, Name = project.Name, Description = project.Description });

    }
    public async Task<ApiResponse<List<ProjectFormResponseDto>>> GetProjectListAsync()
    {
        var projects = await _dbContext.Projects
            .AsNoTracking()
            .Select(project => new ProjectFormResponseDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                IsActive = project.IsActive
            })
            .ToListAsync();

        return ApiResponse<List<ProjectFormResponseDto>>.Success(
            projects,
            "Projects loaded successfully");
    }

    public async Task<ApiResponse<ProjectFormResponseDto>> GetProjectAsync(int id)
    {
        if (id <= 0)
        {
            return ApiResponse<ProjectFormResponseDto>.Fail("The project ID is invalid.");
        }

        var project = await _dbContext.Projects
            .AsNoTracking()
            .Where(project => project.Id == id)
            .Select(project => new ProjectFormResponseDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                IsActive = project.IsActive
            })
            .FirstOrDefaultAsync();

        return project is null
            ? ApiResponse<ProjectFormResponseDto>.Fail("Project not found.")
            : ApiResponse<ProjectFormResponseDto>.Success(
                project,
                "Project loaded successfully");
    }

}
