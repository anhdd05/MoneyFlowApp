using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects
{
    [Table("Users")]
    public class User
    {
        [Key]
        [Column("Id")] // Match với cột Id trong SQL của ông
        public int UserId { get; set; }

        [Column("full_name")]
        public string? FullName { get; set; }

        [Column("Email")]
        public string Email { get; set; } = null!;

        [Column("PasswordHash")] // Match với cột PasswordHash trong SQL của ông
        public string Password { get; set; } = null!;

        [Column("role")]
        public string? Role { get; set; }


        [Column("created_at")]
        public DateTime? CreatedAt { get; set; } 
        public string? ResetToken { get; set; }
        public DateTime? ResetTokenExpiry { get; set; }
    }
}