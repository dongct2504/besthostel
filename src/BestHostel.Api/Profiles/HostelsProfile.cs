using AutoMapper;
using BestHostel.Domain.Entities;
using BestHostel.Domain.Dtos;

namespace BestHostel.Api.Profiles;

public class BestHostelProfile : Profile
{
    public BestHostelProfile()
    {
        // Source -> target
        CreateMap<Hostel, HostelReadDto>().ReverseMap(); // no need to map hostelDto to hostel
        CreateMap<Hostel, HostelCreateDto>().ReverseMap();
        CreateMap<Hostel, HostelUpdateDto>().ReverseMap();

        CreateMap<HostelNumber, HostelNumberReadDto>().ReverseMap();
        CreateMap<HostelNumber, HostelNumberCreateDto>().ReverseMap();
        CreateMap<HostelNumber, HostelNumberUpdateDto>().ReverseMap();
    }
}
