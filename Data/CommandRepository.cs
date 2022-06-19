using Microsoft.EntityFrameworkCore;
using MinApi.Models;

namespace MinApi.Data
{
  public class CommandRepository : ICommandRepository
  {
    private readonly AppDbContext _context;

    public CommandRepository(AppDbContext context)
    {
      _context = context;
    }

    public async Task CreateCommand(Command command)
    {
      if (command == null)
      {
        throw new ArgumentNullException(nameof(command));
      }

      await _context.Commands.AddAsync(command);
    }

    public void Delete(Command command)
    {
      if (command == null)
      {
        throw new ArgumentNullException(nameof(command));
      }

      _context.Commands.Remove(command);
    }

    public async Task<Command?> GetCommandById(int id)
    {
      return await _context.Commands.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Command>> GetCommands()
    {
      return await _context.Commands.ToListAsync();
    }

    public async Task SaveChanges()
    {
      await _context.SaveChangesAsync();
    }
  }
}