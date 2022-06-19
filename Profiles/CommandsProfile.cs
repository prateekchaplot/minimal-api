using AutoMapper;
using MinApi.Dtos;
using MinApi.Models;

namespace MinApi.Profiles
{
  public class CommandsProfile : Profile
  {
    public CommandsProfile()
    {
      CreateMap<Command, CommandReadDto>();
      CreateMap<CommandCreateDto, Command>();
      CreateMap<CommandUpdateDto, Command>();
    }
  }
}