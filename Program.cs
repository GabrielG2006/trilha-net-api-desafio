using System;
using System.Linq;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Models;

var builder = WebApplication.CreateBuilder(args);


var conexao = builder.Configuration.GetConnectionString("ConexaoPadrao");
if (!string.IsNullOrWhiteSpace(conexao) && !conexao.Contains("COLOCAR"))
{
    builder.Services.AddDbContext<OrganizadorContext>(options =>
        options.UseSqlServer(conexao));
}
else
{
  
    builder.Services.AddDbContext<OrganizadorContext>(options =>
        options.UseInMemoryDatabase("TrilhaDb"));
}

builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<OrganizadorContext>();
    if (!context.Tarefas.Any())
    {
        context.Tarefas.AddRange(
            new Tarefa { Titulo = "Exemplo 1", Descricao = "Tarefa de exemplo 1", Data = DateTime.Now, Status = EnumStatusTarefa.Pendente },
            new Tarefa { Titulo = "Exemplo 2", Descricao = "Tarefa de exemplo 2", Data = DateTime.Now.AddDays(1), Status = EnumStatusTarefa.Finalizado }
        );
        context.SaveChanges();
    }
}

app.Run();
