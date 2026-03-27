using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject.Models;

public partial class TransactionCategory
{
    public int CategoryId { get; set; }

    public string Name { get; set; } = null!;

    public string? Type { get; set; }

    public string? Icon { get; set; }

    public bool? IsDeleted { get; set; }
    [Column("color")]
    [StringLength(20)]
    public string? Color { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
