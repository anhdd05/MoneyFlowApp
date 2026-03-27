using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class Budget
{
    public int BudgetId { get; set; }

    public int? UserId { get; set; }

    public int? CategoryId { get; set; }

    public decimal Amount { get; set; }

    public int? Month { get; set; }

    public int? Year { get; set; }

    public string? Description { get; set; }

    public bool? AutoRenew { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual BudgetCategory? Category { get; set; }

    public virtual User? User { get; set; }

    public decimal Allocated { get; set; }
}
