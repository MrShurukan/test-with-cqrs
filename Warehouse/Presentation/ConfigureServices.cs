using System.Reflection;
using Application.Infrastructure.Context;
using Application.Infrastructure.ZoneInfrastructure;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Presentation.Filters;
using SharedClasses.Infrastructure;
using ZoneWriteRepository = Application.Features.ZoneFeatures.ZoneWriteRepository;

namespace Presentation;

public static class ConfigureServices
{
    public static void Configure(WebApplicationBuilder builder)
    {
        AddApplicationServices(builder.Services);
        AddInfrastructureServices(builder.Services);
        AddPresentationServices(builder.Services);

        AddRepositories(builder.Services);
    }

    private static void AddApplicationServices(IServiceCollection services)
    {
        var executingAssembly = Assembly.GetExecutingAssembly();

        services.AddMediatR(executingAssembly);
        services.AddValidatorsFromAssembly(executingAssembly);
        
        // Pipeline
    }

    private static void AddInfrastructureServices(IServiceCollection services)
    {
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();

        services.AddDbContext<ApplicationContext>(opt =>
        {
            opt.UseNpgsql(File.ReadAllText(".dbdata"), 
                builder => builder.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName));
        });
    }

    private static void AddPresentationServices(IServiceCollection services)
    {
        services.AddHttpContextAccessor();
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<ZoneWriteRepository>();
        services.AddScoped<ZoneAggregateRepository>();
        services.AddScoped<ZoneReadRepository>();
    }

    private static void AddPipelineBehaviors(IServiceCollection services)
    {
        services.AddControllers(options => options.Filters.Add<ApiExceptionFilter>());
        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();
    }
}