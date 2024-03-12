using System.ComponentModel.DataAnnotations;

namespace BestHostel.Domain.Dtos;

public class HostelNumberCreateDto
{
    [Required]
    public int HostelNo { get; set; }

    [Required]
    public int HostelId { get; set; }

    public string SpecialDetails { get; set; } = string.Empty;
}
