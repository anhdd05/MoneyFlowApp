using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects;

public class BudgetCategory
{
    [Key]
    [Column("category_id")]
    public int CategoryId { get; set; }

    [Column("name")]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Column("type")]
    [StringLength(50)]
    public string? Type { get; set; }       // "expense" hoặc "saving"

    [Column("icon")]
    public string? Icon { get; set; }

    [Column("is_deleted")]
    public bool? IsDeleted { get; set; }
}