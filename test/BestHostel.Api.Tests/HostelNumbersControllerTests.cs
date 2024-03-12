using System.Linq.Expressions;
using AutoMapper;
using BestHostel.Api.Profiles;
using BestHostel.Domain.Dtos;
using BestHostel.Domain.Entities;
using BestHostel.Domain.Interfaces;
using BestHostel.Infrastructure.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace BestHostel.Api.Tests;

public class HostelNumbersControllerTests : IDisposable
{
    private Mock<IHostelNumberRepository>? _mockHostelNumberRepo;
    private Mock<ILogger<HostelNumbersController>>? _mockHostelNumberLogger;

    private BestHostelProfile? _realHostelsProfile;
    private IMapper? _realMapper;

    public HostelNumbersControllerTests()
    {
        _mockHostelNumberRepo = new Mock<IHostelNumberRepository>();
        _mockHostelNumberLogger = new Mock<ILogger<HostelNumbersController>>();

        _realHostelsProfile = new BestHostelProfile();
        var mapperConfiguration = new MapperConfiguration(config =>
            config.AddProfile(_realHostelsProfile));
        _realMapper = new Mapper(mapperConfiguration);
    }

    public void Dispose()
    {
        _mockHostelNumberRepo = null;
        _mockHostelNumberLogger = null;

        _realHostelsProfile = null;
        _realMapper = null;
    }

    [Fact]
    public async Task GetAllHostelNumbers_Returns200Ok_WhenValidObjectSubmitted()
    {
        // Arrange
        _mockHostelNumberRepo?.Setup(h => h.GetAllAsync(It.IsAny<Expression<Func<HostelNumber, bool>>>()))
            .ReturnsAsync(GetHostelNumbers(1));

        var controller = new HostelNumbersController(_mockHostelNumberRepo?.Object!,
            _mockHostelNumberLogger?.Object!, _realMapper!);

        // Act
        var result = await controller.GetAllHostelNumbers();
        var okResult = result.Result as OkObjectResult;
        var apiResponse = okResult?.Value as ApiResponse;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(okResult);
        Assert.Equal(System.Net.HttpStatusCode.OK, apiResponse?.StatusCode);
    }

    [Fact]
    public async Task GetHostelNumber_Returns200Ok_WhenValidIdSubmitted()
    {
        // Arrange
        _mockHostelNumberRepo?.Setup(h => h.GetAsync(h => h.HostelNo == 100, It.IsAny<bool>()))
            .ReturnsAsync(GetHostelNumber());

        var controller = new HostelNumbersController(_mockHostelNumberRepo?.Object!,
            _mockHostelNumberLogger?.Object!, _realMapper!);

        // Act
        var result = await controller.GetHostelNumber(100);
        var okResult = result.Result as OkObjectResult;
        var apiResponse = okResult?.Value as ApiResponse;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(okResult);
        Assert.Equal(System.Net.HttpStatusCode.OK, apiResponse?.StatusCode);
    }

    [Fact]
    public async Task CreateHostelNumber_Returns201Created_WhenValidObjectSubmitted()
    {
        // Arrange
        _mockHostelNumberRepo?.Setup(h => h.GetAsync(h => h.HostelNo == 100, It.IsAny<bool>()))
            .ReturnsAsync(GetHostelNumber());

        var controller = new HostelNumbersController(_mockHostelNumberRepo?.Object!,
            _mockHostelNumberLogger?.Object!, _realMapper!);

        // Act
        var result = await controller.CreateHostelNumber(new HostelNumberCreateDto { });
        var createdResult = result.Result as CreatedAtRouteResult;
        var apiResponse = createdResult?.Value as ApiResponse;

        // Assert
        Assert.NotNull(createdResult);
        Assert.NotNull(apiResponse);
        Assert.Equal(System.Net.HttpStatusCode.Created, apiResponse.StatusCode);
    }

    [Fact]
    public async Task FullUpdateHostelNumber_Returns201Created_WhenValidObjectSubmitted()
    {
        // Arrange
        _mockHostelNumberRepo?.Setup(h => h.GetAsync(h => h.HostelNo == 100, It.IsAny<bool>()))
            .ReturnsAsync(GetHostelNumber());

        var controller = new HostelNumbersController(_mockHostelNumberRepo?.Object!,
            _mockHostelNumberLogger?.Object!, _realMapper!);

        // Act
        var result = await controller.CreateHostelNumber(new HostelNumberCreateDto { });
        var createdResult = result.Result as CreatedAtRouteResult;
        var apiResponse = createdResult?.Value as ApiResponse;

        // Assert
        Assert.NotNull(createdResult);
        Assert.NotNull(apiResponse);
        Assert.Equal(System.Net.HttpStatusCode.Created, apiResponse.StatusCode);
    }

    [Fact]
    public async Task FullUpdateHostelNumber_Returns204NoContent_WhenValidIdSubmitted()
    {
        // Arrange
        _mockHostelNumberRepo?.Setup(h => h.GetAsync(h => h.HostelNo == 100, It.IsAny<bool>()))
            .ReturnsAsync(GetHostelNumber());

        var controller = new HostelNumbersController(_mockHostelNumberRepo?.Object!,
            _mockHostelNumberLogger?.Object!, _realMapper!);

        // Act
        var result = await controller.FullUpdateHostelNumber(100, new HostelNumberUpdateDto());
        var okResult = result.Result as OkObjectResult;
        var apiResponse = okResult?.Value as ApiResponse;

        // Assert
        Assert.NotNull(okResult);
        Assert.NotNull(apiResponse);
        Assert.Equal(System.Net.HttpStatusCode.NoContent, apiResponse.StatusCode);
    }

    [Fact]
    public async Task DeleteHostelNumber_Returns204NoContent_WhenValidIdSubmitted()
    {
        // Arrange
        _mockHostelNumberRepo?.Setup(h => h.GetAsync(h => h.HostelNo == 100, It.IsAny<bool>()))
            .ReturnsAsync(GetHostelNumber());

        var controller = new HostelNumbersController(_mockHostelNumberRepo?.Object!,
            _mockHostelNumberLogger?.Object!, _realMapper!);

        // Act
        var result = await controller.DeleteHostelNumber(100);
        var okResult = result.Result as OkObjectResult;
        var apiResponse = okResult?.Value as ApiResponse;

        // Assert
        Assert.NotNull(okResult);
        Assert.NotNull(apiResponse);
        Assert.Equal(System.Net.HttpStatusCode.NoContent, apiResponse.StatusCode);
    }

    private IEnumerable<HostelNumber> GetHostelNumbers(int id)
    {
        List<HostelNumber> hostelNumbers = new List<HostelNumber>();

        if (id > 0)
        {
            hostelNumbers.Add(new HostelNumber
            {
                HostelNo = 100,
                HostelId = 1,
                SpecialDetails = "Mock special details"
            });
        }

        return hostelNumbers;
    }

    private HostelNumber GetHostelNumber()
    {
        return new HostelNumber
        {
            HostelNo = 100,
            HostelId = 1,
            SpecialDetails = "Mock special details"
        };
    }
}
