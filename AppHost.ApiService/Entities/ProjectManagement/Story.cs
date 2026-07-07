namespace AppHost.ApiService.Entities.ProjectManagement;

using AppHost.ApiService.Entities.Auth;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table(TableName, Schema = SchemaName)]
public class Story
{
    public const string SchemaName = "ProjectManagement";
    public const string TableName = "Stories";

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(32)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(256)]
    public string Description { get; set; } = string.Empty;

    public int OwnerId { get; set; }

    public User Owner { get; set; } = null!;

    public int ProjectId { get; set; }

    public Projects Project { get; set; } = null!;

    public int? AssignedId { get; set; }

    public User? AssignedUser { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset? UpdatedAtUtc { get; set; }
}
