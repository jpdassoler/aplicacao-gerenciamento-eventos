using Microsoft.EntityFrameworkCore;
using EventManagerBackend.Models;
using DotNetEnv;

// Carrega as variáveis de ambiente do arquivo .env
Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Configurar o Entity Framework Core com MySQL
var connectionString = $"Server={Environment.GetEnvironmentVariable("MYSQL_DB_HOST")};" +
                       $"Database={Environment.GetEnvironmentVariable("MYSQL_DB_NAME")};" +
                       $"User={Environment.GetEnvironmentVariable("MYSQL_DB_USER")};" +
                       $"Password={Environment.GetEnvironmentVariable("MYSQL_DB_PASSWORD")};";
builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
