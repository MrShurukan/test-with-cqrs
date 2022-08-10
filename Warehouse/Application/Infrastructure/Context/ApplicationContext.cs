using Microsoft.EntityFrameworkCore;

namespace Application.Infrastructure.Context;

public class ApplicationContext : DbContext
{
    public DbSet<Zone> Zones { get; set; }
    public DbSet<Container> Containers { get; set; } 
}