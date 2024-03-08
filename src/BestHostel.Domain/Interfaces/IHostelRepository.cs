namespace BestHostel.Domain.Interfaces;

public interface IHostelRepository
{
    Task<IEnumerable<Hostel>> GetAllHostelsAsync();
    Task<Hostel?> GetHostelByIdAsync(int id);
    Task<Hostel?> GetHostelByNameAsync(string hostelName);

    void CreateHostel(Hostel hostel);
    void UpdateHostel(Hostel hostel);
    void DeleteHostel(Hostel hostel);

    Task<bool> SaveChangesAsync();
}
