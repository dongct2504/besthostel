using AutoMapper;
using BestHostel.Domain.Dtos;
using BestHostel.Domain.Entities;
using BestHostel.Domain.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BestHostel.Api;

[ApiController]
[Route("api/hostels")]
public class HostelsController : ControllerBase
{
    private readonly IHostelRepository _hostelRepository;
    private readonly ILogger<HostelsController> _logger;
    private readonly IMapper _mapper;

    public HostelsController(IHostelRepository hostelRepository,
        ILogger<HostelsController> logger,
        IMapper mapper)
    {
        _hostelRepository = hostelRepository;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<HostelReadDto>>> GetAllHostels()
    {
        IEnumerable<Hostel> hostels = await _hostelRepository.GetAllAsync();

        _logger.LogInformation("Getting all hostels");

        return Ok(_mapper.Map<IEnumerable<HostelReadDto>>(hostels));
    }

    [HttpGet("{id:int}", Name = "GetHostelById")]
    public async Task<ActionResult<HostelReadDto>> GetHostelById(int id)
    {
        if (id == 0)
        {
            _logger.LogError($"Get hostel error with id: {id}");
            return BadRequest();
        }

        Hostel? hostelFromDb = await _hostelRepository.GetAsync(h => h.HostelId == id);
        if (hostelFromDb == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<HostelReadDto>(hostelFromDb));
    }

    [HttpPost]
    public async Task<ActionResult<HostelReadDto>> CreateHostel([FromBody] HostelCreateUpdateDto hostelCreateDto)
    {
        if (await _hostelRepository.GetAsync(h => h.Name == hostelCreateDto.Name) != null) // already exist in db
        {
            // key, value
            ModelState.AddModelError("CustomErrorName", "Nhà trọ đã tồn tại.");
            return BadRequest(ModelState);
        }

        if (hostelCreateDto == null)
        {
            return BadRequest();
        }

        Hostel hostel = _mapper.Map<Hostel>(hostelCreateDto);

        await _hostelRepository.CreateAsync(hostel);

        HostelReadDto hostelReadDto = _mapper.Map<HostelReadDto>(hostel);

        return CreatedAtRoute(nameof(GetHostelById), new { Id = hostelReadDto.HostelId }, hostelReadDto);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> FullUpdateHostel(int id, [FromBody] HostelCreateUpdateDto hostelPutDto)
    {
        if (id == 0)
        {
            _logger.LogError($"Get hostel error with id: {id}");
            return BadRequest();
        }

        Hostel? hostelFromDb = await _hostelRepository.GetAsync(filter: h => h.HostelId == id, tracked: false);
        if (hostelFromDb == null)
        {
            return NotFound();
        }

        // Source -> Target
        _mapper.Map(hostelPutDto, hostelFromDb);

        await _hostelRepository.UpdateAsnc(hostelFromDb);

        return NoContent();
    }

    [HttpPatch("{id:int}")]
    public async Task<ActionResult> PartialUpdateHostel(int id,
        JsonPatchDocument<HostelCreateUpdateDto> jsonPatchDoc)
    {
        if (id == 0)
        {
            _logger.LogError($"Get hostel error with id: {id}");
            return BadRequest();
        }

        Hostel? hostelFromDb = await _hostelRepository.GetAsync(h => h.HostelId == id);
        if (hostelFromDb == null)
        {
            return NotFound();
        }

        HostelCreateUpdateDto hostelToPatchDto = _mapper.Map<HostelCreateUpdateDto>(hostelFromDb);

        // validate
        jsonPatchDoc.ApplyTo(hostelToPatchDto, ModelState);

        if (!TryValidateModel(hostelToPatchDto))
        {
            return ValidationProblem(ModelState);
        }

        _mapper.Map(hostelToPatchDto, hostelFromDb);

        // Automatically track so no need to use _hostelRepository.UpdateAsync(hostelFromDb)
        await _hostelRepository.SaveAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteHostel(int id)
    {
        if (id == 0)
        {
            _logger.LogError($"Get hostel error with id: {id}");
            return BadRequest();
        }

        Hostel? hostelFromDb = await _hostelRepository.GetAsync(h => h.HostelId == id);
        if (hostelFromDb == null)
        {
            return NotFound();
        }

        await _hostelRepository.RemoveAsync(hostelFromDb);

        return NoContent();
    }
}