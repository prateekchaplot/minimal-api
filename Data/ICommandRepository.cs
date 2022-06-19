using MinApi.Models;

namespace MinApi.Data
{
  public interface ICommandRepository
  {
    Task SaveChanges();
    Task<Command?> GetCommandById(int id);
    Task<IEnumerable<Command>> GetCommands();
    Task CreateCommand(Command command);
    
    void Delete(Command command);
  }
}