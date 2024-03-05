using AutoMapper;
using BestHostel.Domain;
using BestHostel.Domain.Dtos;

namespace BestHostel.Api.Profiles;

public class HostelsProfile : Profile
{
    public HostelsProfile()
    {
        // Source -> target
        CreateMap<Hostel, HostelReadDto>();
        CreateMap<HostelReadDto, Hostel>();

        CreateMap<Hostel, HostelCreateUpdateDto>();
        CreateMap<HostelCreateUpdateDto, Hostel>();
    }
}
