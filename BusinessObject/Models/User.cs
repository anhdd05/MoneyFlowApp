using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject.Models;

public class User
{
    // Giữ nguyên các ID và các trường bắt buộc
    public int UserId { get; set; }

    // Sửa các trường này thành Nullable để tránh lỗi "Data is Null" khi Login
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? FullName { get; set; }
    public string? Role { get; set; }

    // Các trường Boolean nên để giá trị mặc định hoặc Nullable
    [Column("is_banned")]
    public bool IsBanned { get; set; } = false;

    // Các trường DateTime đã là Nullable, giữ nguyên
    public DateTime? CreatedAt { get; set; }
    public DateTime? LastActive { get; set; }
    public DateTime? ResetTokenExpiry { get; set; }
    public string? ResetToken { get; set; }

    // Navigation properties giữ nguyên
    public virtual ICollection<Budget> Budgets { get; set; } = new List<Budget>();
    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}