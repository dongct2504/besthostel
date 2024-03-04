using BestHostel.Domain;
using BestHostel.Domain.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace BestHostel.Api;

[ApiController]
[Route("api/hostels")]
public class HostelsController : ControllerBase
{
    private readonly ILogger<HostelsController> _logger;

    public HostelsController(ILogger<HostelsController> logger)
    {
        _logger = logger;
    }

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
            _logger.LogError($"GetHostel error with id: {id}");
            return BadRequest();
        }

        return new HostelDto { HostelId = 11, Name = "Hostel11", Address = "123" };
    }
}