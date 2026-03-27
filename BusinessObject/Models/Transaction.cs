using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class Transaction
{
    public int TransactionId { get; set; }

    public int? UserId { get; set; }

    public int? CategoryId { get; set; }

    public decimal Amount { get; set; }

    public DateOnly Date { get; set; }

    public string? Note { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual TransactionCategory? Category { get; set; }

    public virtual User? User { get; set; }
}
