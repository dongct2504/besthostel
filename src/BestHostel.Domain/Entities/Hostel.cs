using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BestHostel.Domain;

public class Hostel
{
    [Key]
    [Required]
    public int HostelId { get; set; }

    [Required]
    [StringLength(128)]
    public string Name { get; set; } = null!;

    [Required]
    public string Description { get; set; } = null!;

    [Required]
    [StringLength(128)]
    public string Address { get; set; } = null!;

    [Required]
    [Column(TypeName = "money")]
    public decimal Price { get; set; }

    [Required]
    [Column(TypeName = "decimal(3, 2)")]
    public decimal DiscountPercent { get; set; }

    [NotMapped]
    public decimal DiscountAmount => Price * DiscountPercent;

    [NotMapped]
    public decimal DiscountPrice => Price - DiscountAmount;
}