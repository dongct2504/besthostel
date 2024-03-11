using AutoMapper;
using BestHostel.Domain.Dtos;
using BestHostel.Domain.Entities;
using BestHostel.Domain.Interfaces;
using BestHostel.Infrastructure.Responses;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BestHostel.Api;

[ApiController]
[Route("api/hostels")]
public class HostelsController : ControllerBase
{
    private readonly ApiResponse _apiResponse;
    private readonly IHostelRepository _hostelRepository;
    private readonly ILogger<HostelsController> _logger;
    private readonly IMapper _mapper;

    public HostelsController(IHostelRepository hostelRepository,
        ILogger<HostelsController> logger,
        IMapper mapper)
    {
        _apiResponse = new ApiResponse();
        _hostelRepository = hostelRepository;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse>> GetAllHostels()
    {
        try
        {
            IEnumerable<Hostel> hostels = await _hostelRepository.GetAllAsync();

            _logger.LogInformation("Getting all hostels");

            _apiResponse.Result = _mapper.Map<IEnumerable<HostelReadDto>>(hostels);
            _apiResponse.StatusCode = System.Net.HttpStatusCode.OK;

            return Ok(_apiResponse);
        }
        catch (Exception e)
        {
            _apiResponse.IsSuccess = false;
            _apiResponse.ErrorMessages = new List<string> { e.ToString() };
        }

        return _apiResponse;
    }

    [HttpGet("{id:int}", Name = "GetHostelById")]
    public async Task<ActionResult<ApiResponse>> GetHostelById(int id)
    {
        try
        {
            if (id == 0)
            {
                _logger.LogError($"Get hostel error with id: {id}");
                _apiResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                return BadRequest(_apiResponse);
            }

            Hostel? hostelFromDb = await _hostelRepository.GetAsync(h => h.HostelId == id);
            if (hostelFromDb == null)
            {
                _apiResponse.StatusCode = System.Net.HttpStatusCode.NotFound;
                return NotFound(_apiResponse);
            }

            _apiResponse.Result = _mapper.Map<HostelReadDto>(hostelFromDb);
            _apiResponse.StatusCode = System.Net.HttpStatusCode.OK;

            return Ok(_apiResponse);
        }
        catch (Exception e)
        {
            _apiResponse.IsSuccess = false;
            _apiResponse.ErrorMessages = new List<string> { e.ToString() };
        }

        return _apiResponse;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse>> CreateHostel([FromBody] HostelCreateDto hostelCreateDto)
    {
        try
        {
            if (await _hostelRepository.GetAsync(h => h.Name == hostelCreateDto.Name) != null)
            {
                // key, value
                ModelState.AddModelError("CustomErrorName", "Nhà trọ đã tồn tại.");
                return BadRequest(ModelState);
            }

            if (hostelCreateDto == null)
            {
                _apiResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                return BadRequest(_apiResponse);
            }

            Hostel hostel = _mapper.Map<Hostel>(hostelCreateDto);

            await _hostelRepository.CreateAsync(hostel);

            _apiResponse.Result = _mapper.Map<HostelReadDto>(hostel);
            _apiResponse.StatusCode = System.Net.HttpStatusCode.Created;

            return CreatedAtRoute(nameof(GetHostelById), new { Id = hostel.HostelId }, _apiResponse);
        }
        catch (Exception e)
        {
            _apiResponse.IsSuccess = false;
            _apiResponse.ErrorMessages = new List<string> { e.ToString() };
        }

        return _apiResponse;
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse>> FullUpdateHostel(int id,
        [FromBody] HostelUpdateDto hostelPutDto)
    {
        try
        {
            if (id == 0)
            {
                _logger.LogError($"Get hostel error with id: {id}");
                _apiResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                return BadRequest(_apiResponse);
            }

            Hostel? hostelFromDb = await _hostelRepository.GetAsync(filter: h => h.HostelId == id, tracked: false);
            if (hostelFromDb == null)
            {
                _apiResponse.StatusCode = System.Net.HttpStatusCode.NotFound;
                return NotFound(_apiResponse);
            }

            // Source -> Target
            _mapper.Map(hostelPutDto, hostelFromDb);

            await _hostelRepository.UpdateAsnc(hostelFromDb);

            _apiResponse.StatusCode = System.Net.HttpStatusCode.NoContent;
            _apiResponse.IsSuccess = true;

            return Ok(_apiResponse);
        }
        catch (Exception e)
        {
            _apiResponse.IsSuccess = false;
            _apiResponse.ErrorMessages = new List<string> { e.ToString() };
        }

        return _apiResponse;
    }

    [HttpPatch("{id:int}")]
    public async Task<ActionResult<ApiResponse>> PartialUpdateHostel(int id,
        JsonPatchDocument<HostelUpdateDto> jsonPatchDoc)
    {
        try
        {
            if (id == 0)
            {
                _logger.LogError($"Get hostel error with id: {id}");
                _apiResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                return BadRequest(_apiResponse);
            }

            Hostel? hostelFromDb = await _hostelRepository.GetAsync(h => h.HostelId == id);
            if (hostelFromDb == null)
            {
                _apiResponse.StatusCode = System.Net.HttpStatusCode.NotFound;
                return NotFound(_apiResponse);
            }

            HostelUpdateDto hostelToPatchDto = _mapper.Map<HostelUpdateDto>(hostelFromDb);

            // validate
            jsonPatchDoc.ApplyTo(hostelToPatchDto, ModelState);

            if (!TryValidateModel(hostelToPatchDto))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(hostelToPatchDto, hostelFromDb);

            // Automatically track so no need to use _hostelRepository.UpdateAsync(hostelFromDb)
            await _hostelRepository.SaveAsync();

            _apiResponse.StatusCode = System.Net.HttpStatusCode.NoContent;
            _apiResponse.IsSuccess = true;

            return Ok(_apiResponse);
        }
        catch (Exception e)
        {
            _apiResponse.IsSuccess = false;
            _apiResponse.ErrorMessages = new List<string> { e.ToString() };
        }

        return _apiResponse;
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse>> DeleteHostel(int id)
    {
        try
        {
            if (id == 0)
            {
                _logger.LogError($"Get hostel error with id: {id}");
                _apiResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                return BadRequest(_apiResponse);
            }

            Hostel? hostelFromDb = await _hostelRepository.GetAsync(h => h.HostelId == id);
            if (hostelFromDb == null)
            {
                _apiResponse.StatusCode = System.Net.HttpStatusCode.NotFound;
                return NotFound(_apiResponse);
            }

            await _hostelRepository.RemoveAsync(hostelFromDb);

            _apiResponse.StatusCode = System.Net.HttpStatusCode.NoContent;
            _apiResponse.IsSuccess = true;

            return Ok(_apiResponse);
        }
        catch (Exception e)
        {
            _apiResponse.IsSuccess = false;
            _apiResponse.ErrorMessages = new List<string> { e.ToString() };
        }

        return _apiResponse;
    }
}