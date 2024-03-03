using BestHostel.Domain;
using BestHostel.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BestHostel.Infrastructure.Persistence;

// to make it handle IdentityUser, use IdentityDbContext
public class BestHostelDbContext : IdentityDbContext
{
    public BestHostelDbContext(DbContextOptions<BestHostelDbContext> options) : base(options)
    {
        
    }

    public DbSet<Hostel> Hostels { get; set; }

    public DbSet<BestHostelUser> BestHostelUsers { get; set; }
}