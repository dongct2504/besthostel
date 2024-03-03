namespace BestHostel.Domain.Dtos;

public class HostelDto
{
    public int HostelId { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Address { get; set; } = null!;

    public decimal Price { get; set; }

    public decimal DiscountPercent { get; set; }

    public decimal DiscountPrice { get; set; }
}