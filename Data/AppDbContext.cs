using Microsoft.EntityFrameworkCore;
using MinApi.Models;

namespace MinApi.Data
{
  public class AppDbContext : DbContext
  {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
      
    }

    public DbSet<Command> Commands => Set<Command>();
  }
}