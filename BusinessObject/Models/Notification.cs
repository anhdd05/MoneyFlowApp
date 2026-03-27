using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class Notification
{
    public int NotificationId { get; set; }

    public int? UserId { get; set; }

    public string? Description { get; set; }

    public DateTime? Date { get; set; }

    public bool? IsRead { get; set; }

    public virtual User? User { get; set; }
}
