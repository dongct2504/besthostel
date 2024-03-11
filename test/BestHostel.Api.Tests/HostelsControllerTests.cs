using AutoMapper;
using BestHostel.Api.Profiles;
using BestHostel.Domain.Entities;
using BestHostel.Domain.Dtos;
using BestHostel.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace BestHostel.Api.Tests;

public class HostelsControllerTests : IDisposable
{
    private Mock<IHostelRepository>? _mockHostelRepository;
    private Mock<ILogger<HostelsController>>? _mockHostelLogger;

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
        _mockHostelLogger = null;
        _hostelsProfile = null;
        _mapper = null;
    }

    // Testing GetAllHostels
    [Fact]
    public async Task GetAllHostels_ReturnsCorrectResultType_WhenValidObjectSubmitted()
    {
        // Note the use of It.* argument matcher to allow for any argument values 
        // to be entered and still get the desired behavior

        // Arrange
        _mockHostelRepository?.Setup(repo => 
                repo.GetAllAsync(It.IsAny<Expression<Func<Hostel, bool>>>()))
            .ReturnsAsync(GetHostels(1));

        HostelsController controller = new HostelsController(
            _mockHostelRepository!.Object, _mockHostelLogger!.Object, _mapper!);

        // Act
        var result = await controller.GetAllHostels();

        // Assert
        Assert.IsType<ActionResult<IEnumerable<HostelReadDto>>>(result.Result);
    }

    // Testing GetHostelById
    [Fact]
    public async Task GetHostel_ReturnsCorrectResultType_WhenValidObjectSubmitted()
    {
        // Arrange
        _mockHostelRepository?.Setup(repo => repo.GetAsync(h => h.HostelId == 1, It.IsAny<bool>()))
            .ReturnsAsync(GetHostel());

        HostelsController controller = new HostelsController(
            _mockHostelRepository!.Object, _mockHostelLogger!.Object, _mapper!);

        // Act
        var result = await controller.GetHostelById(1);

        // Assert
        Assert.IsType<ActionResult<HostelReadDto>>(result);
    }

    [Fact]
    public async Task GetHostel_Returns200Ok_WhenValidIdProvide()
    {
        // Arrange
        _mockHostelRepository?.Setup(repo => repo.GetAsync(h => h.HostelId == 1, It.IsAny<bool>()))
            .ReturnsAsync(GetHostel());

        HostelsController controller = new HostelsController(
            _mockHostelRepository!.Object, _mockHostelLogger!.Object, _mapper!);

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
        _mockHostelRepository?.Setup(repo => repo.GetAsync(h => h.HostelId == 1, It.IsAny<bool>()))
            .ReturnsAsync(GetHostel());

        HostelsController controller = new HostelsController(
            _mockHostelRepository!.Object, _mockHostelLogger!.Object, _mapper!);

        // Act
        var result = await controller.CreateHostel(new HostelCreateDto { });

        // Assert
        Assert.IsType<ActionResult<HostelReadDto>>(result);
    }

    [Fact]
    public async Task CreateHostel_Returns201Created_WhenValidObjectSubmitted()
    {
        // Arrange
        _mockHostelRepository?.Setup(repo => repo.GetAsync(h => h.HostelId == 1, It.IsAny<bool>()))
            .ReturnsAsync(GetHostel());

        HostelsController controller = new HostelsController(
            _mockHostelRepository!.Object, _mockHostelLogger!.Object, _mapper!);

        // Act
        var result = await controller.CreateHostel(new HostelCreateDto { });

        // Assert
        Assert.IsType<CreatedAtRouteResult>(result.Result);
    }

    // [Fact]
    // public async Task CreateHostel_ReturnsCustomError_WhenCreateAnExistantHostel()
    // {
    //     // Arrange
    //     _mockHostelRepository?.Setup(repo => repo.GetAsync(h => h.Name == "ExistantHostel", It.IsAny<bool>()))
    //         .ReturnsAsync(GetHostel());

    //     HostelsController controller = new HostelsController(
    //         _mockHostelRepository!.Object, _mockHostelLogger!.Object, _mapper!);

    //     // Act
    //     var result = await controller.CreateHostel(new HostelCreateUpdateDto { Name = "ExistantHostel" });

    //     // Assert
    //     var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
    //     var error = Assert.IsType<SerializableError>(badRequestResult.Value);
    //     var errorMessages = (string[])error["CustomErrorName"];
    //     var errorMessage = errorMessages[0];

    //     Assert.Equal("Nhà trọ đã tồn tại.", errorMessage);
    // }

    // Testing FullUpdateHostel
    [Fact]
    public async Task FullUpdateHostel_Returns204NoContent_WhenValidObjectSubmitted()
    {
        // Arrange
        _mockHostelRepository?.Setup(repo => repo.GetAsync(h => h.HostelId == 1, It.IsAny<bool>()))
            .ReturnsAsync(GetHostel());

        HostelsController controller = new HostelsController(
            _mockHostelRepository!.Object, _mockHostelLogger!.Object, _mapper!);

        // Act
        var result = await controller.FullUpdateHostel(1, new HostelUpdateDto { });

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task FullUpdateHostel_Returns404NotFound_WhenNonExistentIdSubmitted()
    {
        // Arrange
        _mockHostelRepository?.Setup(repo => repo.GetAsync(h => h.HostelId == 1, It.IsAny<bool>()))
            .ReturnsAsync(GetHostel());

        HostelsController controller = new HostelsController(
            _mockHostelRepository!.Object, _mockHostelLogger!.Object, _mapper!);

        // Act
        var result = await controller.FullUpdateHostel(2, new HostelUpdateDto { });

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task FullUpdateHostel_Returns400BadRequest_WhenNonExistentIdSubmitted()
    {
        // Arrange
        _mockHostelRepository?.Setup(repo => repo.GetAsync(h => h.HostelId == 1, It.IsAny<bool>()))
            .ReturnsAsync(GetHostel());

        HostelsController controller = new HostelsController(
            _mockHostelRepository!.Object, _mockHostelLogger!.Object, _mapper!);

        // Act
        var result = await controller.FullUpdateHostel(0, new HostelUpdateDto { });

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    // Testing PartialUpdateHostel
    [Fact]
    public async Task PartialUpdateHostel_Returns404NotFound_WhenNonExistentIdSubmitted()
    {
        // Arrange
        _mockHostelRepository?.Setup(repo => repo.GetAsync(h => h.HostelId == 1, It.IsAny<bool>()))
            .ReturnsAsync(GetHostel());

        HostelsController controller = new HostelsController(
            _mockHostelRepository!.Object, _mockHostelLogger!.Object, _mapper!);

        // Act
        var result = await controller.PartialUpdateHostel(2,
            new Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<HostelUpdateDto> { });

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    // Testing PartialUpdateHostel
    [Fact]
    public async Task DeleteHostel_Returns204NoContent_WhenNonExistentIdSubmitted()
    {
        // Arrange
        _mockHostelRepository?.Setup(repo => repo.GetAsync(h => h.HostelId == 1, It.IsAny<bool>()))
            .ReturnsAsync(GetHostel());

        HostelsController controller = new HostelsController(
            _mockHostelRepository!.Object, _mockHostelLogger!.Object, _mapper!);

        // Act
        var result = await controller.DeleteHostel(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteHostel_Returns404NotFound_WhenNonExistentIdSubmitted()
    {
        // Arrange
        _mockHostelRepository?.Setup(repo => repo.GetAsync(h => h.HostelId == 1, It.IsAny<bool>()))
            .ReturnsAsync(GetHostel());

        HostelsController controller = new HostelsController(
            _mockHostelRepository!.Object, _mockHostelLogger!.Object, _mapper!);

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
