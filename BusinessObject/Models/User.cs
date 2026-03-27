using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject.Models;

public class User
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Role { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? LastActive { get; set; }

    public virtual ICollection<Budget> Budgets { get; set; } = new List<Budget>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    public string? FullName { get; set; }
    public DateTime? ResetTokenExpiry { get; set; }
    public string? ResetToken { get; set; }
    [Column("last_active", TypeName = "datetime")]
    public DateTime? LastLoginAt { get; set; }

    [Column("is_banned")]
    public bool IsBanned { get; set; }
}
