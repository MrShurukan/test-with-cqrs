using Application.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationContext>(opt =>
{
    opt.UseNpgsql(File.ReadAllText(".dbdata"));
});

var app = builder.Build();

app.Run();