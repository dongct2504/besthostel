using System.ComponentModel.DataAnnotations;

namespace BestHostel.Domain.Dtos;

public class HostelNumberUpdateDto
{
    [Required]
    public int HostelNo { get; set; }

    public string SpecialDetails { get; set; } = string.Empty;
}
