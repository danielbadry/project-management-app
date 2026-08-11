using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos.Story;

public class StoryFromRequestDto
{
    [Required]
    [MaxLength(32)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(256)]
    public string Description { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Please select a project.")]
    public int ProjectId { get; set; }

    [Range(1, int.MaxValue)]
    public int? AssignedId { get; set; }

    public bool IsActive { get; set; } = true;
}
