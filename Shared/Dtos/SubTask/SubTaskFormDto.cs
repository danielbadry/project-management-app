using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos.SubTask;

public class SubTaskFormDto
{
    [Required]
    [MaxLength(32)]
    public string Title { set; get; } = string.Empty;

    [Required]
    [MaxLength(256)]
    public string Description { set; get; } = string.Empty;

    [Required]
    public int StoryId { set; get; }
}