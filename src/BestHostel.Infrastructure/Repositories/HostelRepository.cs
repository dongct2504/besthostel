using BestHostel.Domain;
using BestHostel.Domain.Interfaces;
using BestHostel.Infrastructure.Persistence;

namespace BestHostel.Infrastructure.Repositories;

public class HostelRepository : Repository<Hostel>, IHostelRepository
{
    private readonly BestHostelDbContext _context;

    public HostelRepository(BestHostelDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task UpdateAsnc(Hostel hostel)
    {
        hostel.UpdateDate = DateTime.Now;

        _context.Update(hostel);
        await _context.SaveChangesAsync();
    }
}
