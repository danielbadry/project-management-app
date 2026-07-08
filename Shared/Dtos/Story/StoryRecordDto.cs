using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos.Story;

public class StoryRecordDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int ProjectId { get; set; }
    public int? AssignedId { get; set; }
    public bool IsActive { set; get; } = true;
}