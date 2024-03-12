using BestHostel.Domain.Entities;
using BestHostel.Domain.Interfaces;
using BestHostel.Infrastructure.Persistence;

namespace BestHostel.Infrastructure.Repositories;

public class HostelNumberRepository : Repository<HostelNumber>, IHostelNumberRepository
{
    private readonly BestHostelDbContext _context;

    public HostelNumberRepository(BestHostelDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task UpdateAsync(HostelNumber hostelNumber)
    {
        hostelNumber.UpdateDate = DateTime.Now;

        _context.Update(hostelNumber);

        await _context.SaveChangesAsync();
    }
}
