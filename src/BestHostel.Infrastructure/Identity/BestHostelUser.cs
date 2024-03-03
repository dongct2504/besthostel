using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace BestHostel.Infrastructure.Identity;

public class BestHostelUser : IdentityUser
{
    [NotMapped]
    public string Role { get; set; } = string.Empty;
}
