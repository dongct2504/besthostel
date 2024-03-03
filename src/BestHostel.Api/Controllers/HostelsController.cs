using BestHostel.Domain;
using BestHostel.Domain.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace BestHostel.Api;

[ApiController]
[Route("api/hostels")]
public class HostelsController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Hostel>> GetHostels()
    {
        return Ok(new List<HostelDto>
        {
            new HostelDto { HostelId = 1, Name = "Hostel1", Address = "123" }
        });
    }

    [HttpGet("{id:int}")]
    public ActionResult<HostelDto> GetHostel(int id)
    {
        if (id == 0)
        {
            return BadRequest();
        }

        return new HostelDto { HostelId = 11, Name = "Hostel11", Address = "123" };
    }
}