using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BestHostel.Domain.Entities;

public class HostelNumber
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int HostelNo { get; set; }

    public int HostelId { get; set; }

    public string SpecialDetails { get; set; } = string.Empty;

    public DateTime CreateDate { get; set; }

    public DateTime UpdateDate { get; set; }

    [ForeignKey("HostelId")]
    [InverseProperty("HostelNumbers")]
    public virtual Hostel Hostel { get; set; } = null!;
}
