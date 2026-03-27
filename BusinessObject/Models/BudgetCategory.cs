using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class BudgetCategory
{
    public int CategoryId { get; set; }

    public string Name { get; set; } = null!;

    public string? Type { get; set; }

    public string? Icon { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<Budget> Budgets { get; set; } = new List<Budget>();
}
