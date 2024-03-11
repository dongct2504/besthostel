using System.Net;

namespace BestHostel.Infrastructure.Responses;

public class ApiResponse
{
    public HttpStatusCode StatusCode { get; set; }

    public bool IsSuccess { get; set; } = true;

    public List<string>? ErrorMessages { get; set; }

    public object Result { get; set; } = null!;
}
