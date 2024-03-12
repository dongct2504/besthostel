using AutoMapper;
using BestHostel.Domain.Dtos;
using BestHostel.Domain.Entities;
using BestHostel.Domain.Interfaces;
using BestHostel.Infrastructure.Responses;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BestHostel.Api;

[ApiController]
[Route("api/hostel-numbers")]
public class HostelNumbersController : ControllerBase
{
    private readonly ApiResponse _apiResponse;
    private readonly IHostelNumberRepository _hostelNumberRepository;
    private readonly ILogger<HostelNumbersController> _logger;
    private readonly IMapper _mapper;

    public HostelNumbersController(IHostelNumberRepository hostelNumberRepository,
        ILogger<HostelNumbersController> logger,
        IMapper mapper)
    {
        _apiResponse = new ApiResponse();
        _hostelNumberRepository = hostelNumberRepository;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse>> GetAllHostelNumbers()
    {
        try
        {
            IEnumerable<HostelNumber> hostelNumbers = await _hostelNumberRepository.GetAllAsync();

            _logger.LogInformation("Get all Hostel Numbers");

            _apiResponse.Result = _mapper.Map<IEnumerable<HostelNumberReadDto>>(hostelNumbers);
            _apiResponse.StatusCode = System.Net.HttpStatusCode.OK;

            return Ok(_apiResponse);
        }
        catch (Exception e)
        {
            _apiResponse.IsSuccess = false;
            _apiResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            _apiResponse.ErrorMessages = new List<string> { e.ToString() };

            return StatusCode(500, _apiResponse);
        }
    }

    [HttpGet("{id:int}", Name = "GetHostelNumber")]
    public async Task<ActionResult<ApiResponse>> GetHostelNumber(int id)
    {
        try
        {
            if (id < 0)
            {
                string errorMessage = $"Get hostel number error with id: {id}";

                _logger.LogError(errorMessage);

                _apiResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = new List<string> { errorMessage };

                return BadRequest(_apiResponse);
            }

            HostelNumber? hostelNumberFromDb = await _hostelNumberRepository.GetAsync(h => h.HostelNo == id);
            if (hostelNumberFromDb == null)
            {
                string errorMessage = $"Not Found hostel number by this Hostel number: {id}";

                _logger.LogError(errorMessage);

                _apiResponse.StatusCode = System.Net.HttpStatusCode.NotFound;
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = new List<string> { errorMessage };

                return NotFound(_apiResponse);
            }

            _apiResponse.Result = _mapper.Map<HostelNumberReadDto>(hostelNumberFromDb);
            _apiResponse.StatusCode = System.Net.HttpStatusCode.OK;

            return Ok(_apiResponse);
        }
        catch (Exception e)
        {
            _apiResponse.IsSuccess = false;
            _apiResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            _apiResponse.ErrorMessages = new List<string> { e.ToString() };

            return StatusCode(500, _apiResponse);
        }
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse>> CreateHostelNumber(
        [FromBody] HostelNumberCreateDto hostelNumberCreateDto)
    {
        try
        {
            if (await _hostelNumberRepository.GetAsync(h => h.HostelNo == hostelNumberCreateDto.HostelNo) != null)
            {
                // key, value
                ModelState.AddModelError("CustomErrorName", "Nhà trọ đã tồn tại.");
                return BadRequest(ModelState);
            }

            if (hostelNumberCreateDto == null)
            {
                string errorMessage = "Can not create hostel number because it is null";

                _apiResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = new List<string> { errorMessage };

                return BadRequest(_apiResponse);
            }

            HostelNumber hostelNumber = _mapper.Map<HostelNumber>(hostelNumberCreateDto);

            await _hostelNumberRepository.CreateAsync(hostelNumber);

            _apiResponse.Result = _mapper.Map<HostelNumberReadDto>(hostelNumber);
            _apiResponse.StatusCode = System.Net.HttpStatusCode.Created;

            return CreatedAtRoute("GetHostelNumber", new { Id = hostelNumber.HostelNo }, _apiResponse);
        }
        catch (Exception e)
        {
            _apiResponse.IsSuccess = false;
            _apiResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            _apiResponse.ErrorMessages = new List<string> { e.ToString() };

            return StatusCode(500, _apiResponse);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse>> FullUpdateHostelNumber(int id,
        [FromBody] HostelNumberUpdateDto hostelNumberUpdateDto)
    {
        try
        {
            if (id < 0)
            {
                string errorMessage = $"Get hostel number error with id: {id}";

                _logger.LogError(errorMessage);

                _apiResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = new List<string> { errorMessage };

                return BadRequest(_apiResponse);
            }

            HostelNumber? hostelNumberFromDb = await _hostelNumberRepository.GetAsync(
                filter: h => h.HostelNo == id, tracked: false);
            if (hostelNumberFromDb == null)
            {
                string errorMessage = $"Not Found hostel number by this id: {id}";

                _logger.LogError(errorMessage);

                _apiResponse.StatusCode = System.Net.HttpStatusCode.NotFound;
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = new List<string> { errorMessage };

                return NotFound(_apiResponse);
            }

            _mapper.Map(hostelNumberUpdateDto, hostelNumberFromDb);

            await _hostelNumberRepository.UpdateAsync(hostelNumberFromDb);

            _apiResponse.StatusCode = System.Net.HttpStatusCode.NoContent;

            return Ok(_apiResponse);
        }
        catch (Exception e)
        {
            _apiResponse.IsSuccess = false;
            _apiResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            _apiResponse.ErrorMessages = new List<string> { e.ToString() };

            return StatusCode(500, _apiResponse);
        }
    }

    [HttpPatch("{id:int}")]
    public async Task<ActionResult<ApiResponse>> PartialUpdateHostelNumber(int id,
        [FromBody] JsonPatchDocument<HostelNumberUpdateDto> jsonPatchDoc)
    {
        try
        {
            if (id < 0)
            {
                string errorMessage = $"Not Found hostel number by this id: {id}";

                _logger.LogError(errorMessage);

                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = new List<string> { errorMessage };
                _apiResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;

                return BadRequest(_apiResponse);
            }

            HostelNumber? hostelNumberFromDb = await _hostelNumberRepository
                .GetAsync(h => h.HostelNo == id, false);
            if (hostelNumberFromDb == null)
            {
                string errorMessage = $"Not Found hostel number by this id: {id}";

                _logger.LogError(errorMessage);

                _apiResponse.StatusCode = System.Net.HttpStatusCode.NotFound;
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = new List<string> { errorMessage };

                return NotFound(_apiResponse);
            }

            HostelNumberUpdateDto hostelNumberToPatchDto = _mapper.Map<HostelNumberUpdateDto>(hostelNumberFromDb);

            jsonPatchDoc.ApplyTo(hostelNumberToPatchDto, ModelState);
            if (!TryValidateModel(ModelState))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(hostelNumberToPatchDto, hostelNumberFromDb);

            await _hostelNumberRepository.UpdateAsync(hostelNumberFromDb);

            _apiResponse.StatusCode = System.Net.HttpStatusCode.NoContent;

            return Ok(_apiResponse);
        }
        catch (Exception e)
        {
            _apiResponse.IsSuccess = false;
            _apiResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            _apiResponse.ErrorMessages = new List<string> { e.ToString() };

            return StatusCode(500, _apiResponse);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse>> DeleteHostelNumber(int id)
    {
        try
        {
            if (id < 0)
            {
                string errorMessage = $"Not Found hostel number by this id: {id}";

                _logger.LogError(errorMessage);

                _apiResponse.IsSuccess = false;
                _apiResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _apiResponse.ErrorMessages = new List<string> { errorMessage };

                return BadRequest(_apiResponse);
            }

            HostelNumber? hostelNumberFromDb = await _hostelNumberRepository.GetAsync(h => h.HostelNo == id);
            if (hostelNumberFromDb == null)
            {
                string errorMessage = $"Not Found hostel number by this id: {id}";

                _logger.LogError(errorMessage);

                _apiResponse.StatusCode = System.Net.HttpStatusCode.NotFound;
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = new List<string> { errorMessage };

                return NotFound(_apiResponse);
            }

            await _hostelNumberRepository.RemoveAsync(hostelNumberFromDb);

            _apiResponse.StatusCode = System.Net.HttpStatusCode.NoContent;

            return Ok(_apiResponse);
        }
        catch (Exception e)
        {
            _apiResponse.IsSuccess = false;
            _apiResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            _apiResponse.ErrorMessages = new List<string> { e.ToString() };

            return StatusCode(500, _apiResponse);
        }
    }
}
