using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos.SubTask;

public class SubTaskRecordDto
{
    public int Id { set; get; }
    public string Title { set; get; } = string.Empty;
    public string Description { set; get; } = string.Empty;
    public int StoryId { set; get; }
    public bool IsActive { set; get; } = true;
}