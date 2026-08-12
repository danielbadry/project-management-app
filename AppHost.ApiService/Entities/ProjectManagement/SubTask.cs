namespace AppHost.ApiService.Entities.ProjectManagement;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table(TableName, Schema = SchemaName)]
public class SubTask
{
    public const string SchemaName = "ProjectManagement";
    public const string TableName = "SubTasks";

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(32)]
    public string Title { set; get; } = string.Empty;

    [Required]
    [MaxLength(256)]
    public string Description { set; get; } = string.Empty;

    [Required]
    public int StoryId { set; get; }

    public Story ParentStory { set; get; } = null!;

    public bool IsActive { get; set; } = true;

    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset? UpdatedAtUtc { get; set; }
}
