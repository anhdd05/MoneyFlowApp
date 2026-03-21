using System;
using System.Collections.Generic;

namespace DataAccessLayer.Models;

public partial class TransactionCategory
{
    public int CategoryId { get; set; }

    public string Name { get; set; } = null!;

    public string? Type { get; set; }

    public string? Icon { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
