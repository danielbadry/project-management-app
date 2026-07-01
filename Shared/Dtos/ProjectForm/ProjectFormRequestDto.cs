using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos.ProjectForm;

public class ProjectFormRequestDto
{
    [Required]
    [MaxLength(32)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(256)]
    public string Description { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}
