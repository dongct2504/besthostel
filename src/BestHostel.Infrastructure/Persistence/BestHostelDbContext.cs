using BestHostel.Domain.Entities;
using BestHostel.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BestHostel.Infrastructure.Persistence;

// to make it handle IdentityUser, use IdentityDbContext
public class BestHostelDbContext : IdentityDbContext<BestHostelUser>
{
    public BestHostelDbContext(DbContextOptions<BestHostelDbContext> options) : base(options)
    {
        
    }

    public DbSet<Hostel> Hostels { get; set; }
    public DbSet<HostelNumber> HostelNumbers { get; set; }

    public DbSet<BestHostelUser> BestHostelUsers { get; set; }
}