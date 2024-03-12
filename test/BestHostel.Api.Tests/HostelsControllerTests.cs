using AutoMapper;
using BestHostel.Api.Profiles;
using BestHostel.Domain.Entities;
using BestHostel.Domain.Dtos;
using BestHostel.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;
using BestHostel.Infrastructure.Responses;

namespace BestHostel.Api.Tests;

public class HostelsControllerTests : IDisposable
{
    private Mock<IHostelRepository>? _mockHostelRepository;
    private Mock<ILogger<HostelsController>>? _mockHostelLogger;

    private BestHostelProfile? _hostelsProfile;
    private IMapper? _mapper;

    public HostelsControllerTests()
    {
        _mockHostelRepository = new Mock<IHostelRepository>();
        _mockHostelLogger = new Mock<ILogger<HostelsController>>();

        // add real mapper
        _hostelsProfile = new BestHostelProfile();

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
    public async Task GetAllHostels_Returns200Ok_WhenValidObjectSubmitted()
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
        var okResult = result.Result as OkObjectResult;
        var apiResponse = okResult?.Value as ApiResponse;

        // Assert
        Assert.NotNull(okResult);
        Assert.NotNull(apiResponse);
        Assert.Equal(System.Net.HttpStatusCode.OK, apiResponse.StatusCode);
    }

    // Testing GetHostelById
    [Fact]
    public async Task GetHostel_Returns200Ok_WhenValidObjectSubmitted()
    {
        // Arrange
        _mockHostelRepository?.Setup(repo => repo.GetAsync(h => h.HostelId == 1, It.IsAny<bool>()))
            .ReturnsAsync(GetHostel());

        HostelsController controller = new HostelsController(
            _mockHostelRepository!.Object, _mockHostelLogger!.Object, _mapper!);

        // Act
        var result = await controller.GetHostelById(1);
        var okResult = result.Result as OkObjectResult;
        var apiResponse = okResult?.Value as ApiResponse;

        // Assert
        Assert.NotNull(okResult);
        Assert.NotNull(apiResponse);
        Assert.Equal(System.Net.HttpStatusCode.OK, apiResponse.StatusCode);
    }

    // Testing CreateHostel
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
        var createdAtRoute = result.Result as CreatedAtRouteResult;
        var apiResponse = createdAtRoute?.Value as ApiResponse;

        // Assert
        Assert.NotNull(createdAtRoute);
        Assert.NotNull(apiResponse);
        Assert.Equal(System.Net.HttpStatusCode.Created, apiResponse.StatusCode);
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
        var okResult = result.Result as OkObjectResult;
        var apiResponse = okResult?.Value as ApiResponse;

        // Assert
        Assert.NotNull(okResult);
        Assert.NotNull(apiResponse);
        Assert.Equal(System.Net.HttpStatusCode.NoContent, apiResponse.StatusCode);
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
        var notFoundResult = result.Result as NotFoundObjectResult;
        var apiResponse = notFoundResult?.Value as ApiResponse;

        // Assert
        Assert.NotNull(notFoundResult);
        Assert.NotNull(apiResponse);
        Assert.Equal(System.Net.HttpStatusCode.NotFound, apiResponse.StatusCode);
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
        var badRequestResult = result.Result as BadRequestObjectResult;
        var apiResponse = badRequestResult?.Value as ApiResponse;

        // Assert
        Assert.NotNull(badRequestResult);
        Assert.NotNull(apiResponse);
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, apiResponse.StatusCode);
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
        var notFoundResult = result.Result as NotFoundObjectResult;
        var apiResponse = notFoundResult?.Value as ApiResponse;

        // Assert
        Assert.NotNull(notFoundResult);
        Assert.NotNull(apiResponse);
        Assert.Equal(System.Net.HttpStatusCode.NotFound, apiResponse.StatusCode);
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
        var okResult = result.Result as OkObjectResult;
        var apiResponse = okResult?.Value as ApiResponse;

        // Assert
        Assert.NotNull(okResult);
        Assert.NotNull(apiResponse);
        Assert.Equal(System.Net.HttpStatusCode.NoContent, apiResponse.StatusCode);
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
        var notFoundResult = result.Result as NotFoundObjectResult;
        var apiResponse = notFoundResult?.Value as ApiResponse;

        // Assert
        Assert.NotNull(notFoundResult);
        Assert.NotNull(apiResponse);
        Assert.Equal(System.Net.HttpStatusCode.NotFound, apiResponse.StatusCode);
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
