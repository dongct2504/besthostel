using BestHostel.Domain.Interfaces;
using BestHostel.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace BestHostel.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IHostelRepository, HostelRepository>();
        services.AddScoped<IHostelNumberRepository, HostelNumberRepository>();

        return services;
    }
}