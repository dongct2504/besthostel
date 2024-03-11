using System.ComponentModel.DataAnnotations;

namespace BestHostel.Domain.Dtos;

public class HostelUpdateDto
{
    [Required]
    [StringLength(128)]
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    [Required]
    public double Rate { get; set; }

    public int Sqft { get; set; }

    public int Occupancy { get; set; }

    public string? ImageUrl { get; set; }

    public string? Amenity { get; set; }

    public string Address { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public decimal DiscountPercent { get; set; }

    public decimal DiscountPrice { get; set; }
}