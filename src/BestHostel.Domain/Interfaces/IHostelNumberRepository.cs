using BestHostel.Domain.Entities;

namespace BestHostel.Domain.Interfaces;

public interface IHostelNumberRepository : IRepository<HostelNumber>
{
    Task UpdateAsync(HostelNumber hostelNumber);
}
