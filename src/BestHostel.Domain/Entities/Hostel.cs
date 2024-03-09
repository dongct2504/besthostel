using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BestHostel.Domain.Entities;

public class Hostel
{
    [Key]
    [Required]
    public int HostelId { get; set; }

    [Required]
    [StringLength(128)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    public double Rate { get; set; }

    [Required]
    public int Sqft { get; set; }

    [Required]
    public int Occupancy { get; set; }

    public string? ImageUrl { get; set; }

    public string? Amenity { get; set; }

    [Required]
    [StringLength(128)]
    public string Address { get; set; } = string.Empty;

    public DateTime CreateDate { get; set; }

    public DateTime UpdateDate { get; set; }

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