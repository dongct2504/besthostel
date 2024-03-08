using AutoMapper;
using BestHostel.Api.Profiles;
using BestHostel.Domain;
using BestHostel.Domain.Dtos;
using BestHostel.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace BestHostel.Api.Tests;

public class HostelsControllerTests : IDisposable
{
    private Mock<IHostelRepository>? _mockHostelRepository;
    private Mock<ILogger<HostelsController>> _mockHostelLogger;

    private HostelsProfile? _hostelsProfile;
    private IMapper? _mapper;

    public HostelsControllerTests()
    {
        _mockHostelRepository = new Mock<IHostelRepository>();
        _mockHostelLogger = new Mock<ILogger<HostelsController>>();

        // add real mapper
        _hostelsProfile = new HostelsProfile();

        var mapperConfiguration = new MapperConfiguration(config =>
            config.AddProfile(_hostelsProfile));

        _mapper = new Mapper(mapperConfiguration);
    }

    public void Dispose()
    {
        _mockHostelRepository = null;
        _hostelsProfile = null;
        _mapper = null;
    }

    // Testing GetAllHostels
    [Fact]
    public async Task GetAllHostels_ReturnsCorrectResultType_WhenValidObjectSubmitted()
    {
        // Arrange
        _mockHostelRepository?.Setup(repo => repo.GetAllHostelsAsync()).ReturnsAsync(GetHostels(1));

        HostelsController controller = new HostelsController(
            _mockHostelRepository!.Object, _mockHostelLogger.Object, _mapper!);

        // Act
        var result = await controller.GetAllHostels();

        // Assert
        Assert.IsType<ActionResult<IEnumerable<HostelReadDto>>>(result);
    }

    // Testing GetHostelById
    [Fact]
    public async Task GetHostel_ReturnsCorrectResultType_WhenValidObjectSubmitted()
    {
        // Arrange
        _mockHostelRepository?.Setup(repo => repo.GetHostelByIdAsync(1)).ReturnsAsync(GetHostel());

        HostelsController controller = new HostelsController(
            _mockHostelRepository!.Object, _mockHostelLogger.Object, _mapper!);

        // Act
        var result = await controller.GetHostelById(1);

        // Assert
        Assert.IsType<ActionResult<HostelReadDto>>(result);
    }

    [Fact]
    public async Task GetHostel_Returns200Ok_WhenValidIdProvide()
    {
        // Arrange
        _mockHostelRepository?.Setup(repo => repo.GetHostelByIdAsync(1)).ReturnsAsync(GetHostel());

        HostelsController controller = new HostelsController(
            _mockHostelRepository!.Object, _mockHostelLogger.Object, _mapper!);

        // Act
        var result = await controller.GetHostelById(1);

        // Assert
        Assert.IsType<OkObjectResult>(result.Result);
    }

    // Testing CreateHostel
    [Fact]
    public async Task CreateHostel_ReturnsCorrectResultType_WhenValidObjectSubmitted()
    {
        // Arrange
        _mockHostelRepository?.Setup(repo => repo.GetHostelByIdAsync(1)).ReturnsAsync(GetHostel());

        HostelsController controller = new HostelsController(
            _mockHostelRepository!.Object, _mockHostelLogger.Object, _mapper!);

        // Act
        var result = await controller.CreateHostel(new HostelCreateUpdateDto { });

        // Assert
        Assert.IsType<ActionResult<HostelReadDto>>(result);
    }

    [Fact]
    public async Task CreateHostel_Returns201Created_WhenValidObjectSubmitted()
    {
        // Arrange
        _mockHostelRepository?.Setup(repo => repo.GetHostelByIdAsync(1)).ReturnsAsync(GetHostel());

        HostelsController controller = new HostelsController(
            _mockHostelRepository!.Object, _mockHostelLogger.Object, _mapper!);

        // Act
        var result = await controller.CreateHostel(new HostelCreateUpdateDto { });

        // Assert
        Assert.IsType<CreatedAtRouteResult>(result.Result);
    }

    [Fact]
    public async Task CreateHostel_ReturnsCustomError_WhenCreateAnExistantHostel()
    {
        // Arrange
        Hostel existantHostel = GetHostel();

        _mockHostelRepository?.Setup(repo => repo.GetHostelByNameAsync("ExistantHostel"))
            .ReturnsAsync(GetHostel());

        HostelsController controller = new HostelsController(
            _mockHostelRepository!.Object, _mockHostelLogger.Object, _mapper!);

        // Act
        var result = await controller.CreateHostel(new HostelCreateUpdateDto { Name = "ExistantHostel" });

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        var error = Assert.IsType<SerializableError>(badRequestResult.Value);
        var errorMessages = (string[])error["CustomErrorName"];
        var errorMessage = errorMessages[0];

        Assert.Equal("Nhà trọ đã tồn tại.", errorMessage);
    }

    // Testing FullUpdateHostel
    [Fact]
    public async Task FullUpdateHostel_Returns204NoContent_WhenValidObjectSubmitted()
    {
        // Arrange
        _mockHostelRepository?.Setup(repo => repo.GetHostelByIdAsync(1)).ReturnsAsync(GetHostel());

        HostelsController controller = new HostelsController(
            _mockHostelRepository!.Object, _mockHostelLogger.Object, _mapper!);

        // Act
        var result = await controller.FullUpdateHostel(1, new HostelCreateUpdateDto { });

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task FullUpdateHostel_Returns404NotFound_WhenNonExistentIdSubmitted()
    {
        // Arrange
        _mockHostelRepository?.Setup(repo => repo.GetHostelByIdAsync(1)).ReturnsAsync(GetHostel());

        HostelsController controller = new HostelsController(
            _mockHostelRepository!.Object, _mockHostelLogger.Object, _mapper!);

        // Act
        var result = await controller.FullUpdateHostel(2, new HostelCreateUpdateDto { });

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task FullUpdateHostel_Returns400BadRequest_WhenNonExistentIdSubmitted()
    {
        // Arrange
        _mockHostelRepository?.Setup(repo => repo.GetHostelByIdAsync(1)).ReturnsAsync(GetHostel());

        HostelsController controller = new HostelsController(
            _mockHostelRepository!.Object, _mockHostelLogger.Object, _mapper!);

        // Act
        var result = await controller.FullUpdateHostel(0, new HostelCreateUpdateDto { });

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    // Testing PartialUpdateHostel
    [Fact]
    public async Task PartialUpdateHostel_Returns404NotFound_WhenNonExistentIdSubmitted()
    {
        // Arrange
        _mockHostelRepository?.Setup(repo => repo.GetHostelByIdAsync(1)).ReturnsAsync(GetHostel());

        HostelsController controller = new HostelsController(
            _mockHostelRepository!.Object, _mockHostelLogger.Object, _mapper!);

        // Act
        var result = await controller.PartialUpdateHostel(2,
            new Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<HostelCreateUpdateDto> { });

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    // Testing PartialUpdateHostel
    [Fact]
    public async Task DeleteHostel_Returns204NoContent_WhenNonExistentIdSubmitted()
    {
        // Arrange
        _mockHostelRepository?.Setup(repo => repo.GetHostelByIdAsync(1)).ReturnsAsync(GetHostel());

        HostelsController controller = new HostelsController(
            _mockHostelRepository!.Object, _mockHostelLogger.Object, _mapper!);

        // Act
        var result = await controller.DeleteHostel(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteHostel_Returns404NotFound_WhenNonExistentIdSubmitted()
    {
        // Arrange
        _mockHostelRepository?.Setup(repo => repo.GetHostelByIdAsync(1)).ReturnsAsync(GetHostel());

        HostelsController controller = new HostelsController(
            _mockHostelRepository!.Object, _mockHostelLogger.Object, _mapper!);

        // Act
        var result = await controller.DeleteHostel(2);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    private IEnumerable<Hostel> GetHostels(int id)
    {
        List<Hostel> hostels = new List<Hostel>();

        if (id > 0)
        {
            hostels.Add(new Hostel
            {
                HostelId = 1,
                Name = "Mock name",
                Description = "Mock Description",
                Rate = 0.02,
                Sqft = 3,
                Occupancy = 8,
                Address = "Mock Address",
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now
            });
        }

        return hostels;
    }

    private Hostel GetHostel()
    {
        return new Hostel
        {
            HostelId = 1,
            Name = "Mock name",
            Description = "Mock Description",
            Rate = 0.02,
            Sqft = 3,
            Occupancy = 8,
            Address = "Mock Address",
            CreateDate = DateTime.Now,
            UpdateDate = DateTime.Now
        };
    }
}
