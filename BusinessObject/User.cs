using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects;

[Table("Users")] // Đảm bảo Entity Framework tìm đúng bảng Users
public class User
{
    [Key]
    [Column("user_id")]
    public int UserId { get; set; }

    [Column("user_name")]
    [StringLength(50)]
    public string UserName { get; set; } = null!;

    [Column("email")]
    [StringLength(100)]
    public string Email { get; set; } = null!;

    [Column("password")]
    [StringLength(255)]
    public string Password { get; set; } = null!;

    [Column("role")]
    [StringLength(20)]
    public string? Role { get; set; }

    [Column("created_at", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    // SỬA TẠI ĐÂY: Thêm Mapping để không bị lỗi "Invalid column name"
    [Column("ResetToken")]
    public string? ResetToken { get; set; }

    [Column("ResetTokenExpiry")]
    public DateTime? ResetTokenExpiry { get; set; }
}