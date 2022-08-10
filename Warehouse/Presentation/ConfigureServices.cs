using Application.Infrastructure.ZoneInfrastructure;
using Core.Entities;
using ZoneWriteRepository = Application.Features.ZoneFeatures.ZoneWriteRepository;

namespace Presentation;

public static class ConfigureServices
{
    public static void Configure(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ZoneWriteRepository>();
        builder.Services.AddScoped<ZoneAggregateRepository>();
        builder.Services.AddScoped<ZoneReadRepository>();
    }
}