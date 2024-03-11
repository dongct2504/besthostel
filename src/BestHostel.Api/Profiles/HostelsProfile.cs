using AutoMapper;
using BestHostel.Domain.Entities;
using BestHostel.Domain.Dtos;

namespace BestHostel.Api.Profiles;

public class HostelsProfile : Profile
{
    public HostelsProfile()
    {
        // Source -> target
        CreateMap<Hostel, HostelReadDto>().ReverseMap(); // no need to map hostelDto to hostel
        CreateMap<Hostel, HostelCreateDto>().ReverseMap();
        CreateMap<Hostel, HostelUpdateDto>().ReverseMap();
    }
}
