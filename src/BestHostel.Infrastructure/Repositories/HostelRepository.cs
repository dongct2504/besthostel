using BestHostel.Domain;
using BestHostel.Domain.Interfaces;
using BestHostel.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BestHostel.Infrastructure.Repositories;

public class HostelRepository : IHostelRepository
{
    private readonly BestHostelDbContext _context;

    public HostelRepository(BestHostelDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Hostel>> GetAllHostelsAsync()
    {
        return await _context.Hostels.ToListAsync();
    }

    public async Task<Hostel?> GetHostelByIdAsync(int id)
    {
        return await _context.Hostels.FindAsync(id);
    }

    public void CreateHostel(Hostel hostel)
    {
        _context.Add(hostel);
    }

    public void UpdateHostel(Hostel hostel)
    {
        _context.Update(hostel);
    }

    public void DeleteHostel(Hostel hostel)
    {
        if (hostel == null)
        {
            throw new ArgumentException(nameof(hostel));
        }

        _context.Remove(hostel);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() >= 0;
    }
}
