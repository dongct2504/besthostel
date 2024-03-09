namespace BestHostel.Domain.Interfaces;

public interface IHostelRepository : IRepository<Hostel>
{
    Task UpdateAsnc(Hostel hostel);
}
