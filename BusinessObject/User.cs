using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects;

[Table("Users")]
public class User
{
    [Key]
    [Column("user_id")]
    public int UserId { get; set; }

    

    [Column("email")]
    public string Email { get; set; } = null!;

    [Column("password")]
    public string Password { get; set; } = null!;

    [Column("created_at", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    public string? ResetToken { get; set; }
    public DateTime? ResetTokenExpiry { get; set; }
}