namespace AppHost.ApiService.Entities.Auth;

using AppHost.ApiService.Entities.ProjectManagement;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table(TableName, Schema = SchemaName)]
public class User
{
    public const string SchemaName = "Auth";
    public const string TableName = "Users";

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(256)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [MaxLength(512)]
    public string PasswordHash { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public int FailedLoginAttempts { get; set; }

    public DateTimeOffset? LockoutEndUtc { get; set; }

    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset? UpdatedAtUtc { get; set; }

    public ICollection<Story> OwnedStories { get; set; } = [];

    public ICollection<Story> AssignedStories { get; set; } = [];
}
