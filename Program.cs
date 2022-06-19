using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MinApi.Data;
using MinApi.Dtos;
using MinApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var sqlConnBuilder = new SqlConnectionStringBuilder()
{
    ConnectionString = builder.Configuration.GetConnectionString("SqlServer"),
    UserID = builder.Configuration["UserId"],
    Password = builder.Configuration["Password"]
};

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(sqlConnBuilder.ConnectionString));

builder.Services.AddScoped<ICommandRepository, CommandRepository>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("api/v1/commands", async (ICommandRepository repository, IMapper mapper) =>
{
    var commands = await repository.GetCommands();
    var commandsRead = mapper.Map<IEnumerable<CommandReadDto>>(commands);
    return Results.Ok(commandsRead);
});

app.MapGet("api/v1/commands/{id}", async (ICommandRepository repository, IMapper mapper, int id) =>
{
    var command = await repository.GetCommandById(id);
    if (command == null)
    {
        return Results.NotFound();
    }

    var commandRead = mapper.Map<CommandReadDto>(command);
    return Results.Ok(commandRead);
});

app.MapPost("api/v1/commands", async (ICommandRepository repository, IMapper mapper, CommandCreateDto commandCreate) =>
{
    var command = mapper.Map<Command>(commandCreate);
    await repository.CreateCommand(command);
    await repository.SaveChanges();

    var commandRead = mapper.Map<CommandReadDto>(command);
    return Results.Created($"api/v1/commands/{command.Id}", commandRead);
});

app.MapPut("api/v1/commands/{id}", async (ICommandRepository repository, IMapper mapper, int id, CommandUpdateDto commandUpdate) =>
{
    var command = await repository.GetCommandById(id);
    if (command == null)
    {
        return Results.BadRequest();
    }
    
    mapper.Map(commandUpdate, command);
    await repository.SaveChanges();
    return Results.NoContent();
});

app.MapDelete("api/v1/commands/{id}", async (ICommandRepository repository, IMapper mapper, int id) =>
{
    var command = await repository.GetCommandById(id);
    if (command == null)
    {
        return Results.BadRequest();
    }

    repository.Delete(command);
    await repository.SaveChanges();
    return Results.NoContent();
});

app.Run();
