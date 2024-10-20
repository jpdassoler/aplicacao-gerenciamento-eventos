using Microsoft.EntityFrameworkCore;
using EventManagerBackend.Models;
using DotNetEnv;
using EventManagerBackend.Services;
using EventManagerBackend.Repositories;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Carrega as variáveis de ambiente do arquivo .env
var projectDirectory = Directory.GetCurrentDirectory();
if (builder.Environment.IsDevelopment())
{
    var envFilePath = Path.Combine(projectDirectory, "Environments", ".env.development");
    Console.WriteLine($"Loading environment variables from: {envFilePath}");
    Env.Load(envFilePath);
} else 
{
    var envFilePath = Path.Combine(projectDirectory, "Environments", ".env.production");
    Env.Load(envFilePath);
}

//Teste com banco de dados de produção, comentar trecho acima e descomentar abaixo
/*
var envFilePath = Path.Combine(projectDirectory, "Environments", ".env.production");
Env.Load(envFilePath);
*/

// Configurar o Entity Framework Core com MySQL
var connectionString = $"Server={Env.GetString("MYSQL_DB_HOST")};" +
                       $"Database={Env.GetString("MYSQL_DB_NAME")};" +
                       $"User={Env.GetString("MYSQL_DB_USER")};" +
                       $"Password={Env.GetString("MYSQL_DB_PASSWORD")};";

builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Add services to the container.
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IEnderecoService, EnderecoService>();
builder.Services.AddScoped<IEnderecoRepository, EnderecoRepository>();
builder.Services.AddScoped<IEventoService, EventoService>();
builder.Services.AddScoped<IEventoRepository, EventoRepository>();
builder.Services.AddScoped<IClienteEventoService, ClienteEventoService>();
builder.Services.AddScoped<IClienteEventoRepository, ClienteEventoRepository>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        builder =>
        {
            var frontEndUrl = Env.GetString("FRONTEND_URL");
            builder.WithOrigins(frontEndUrl)
                    .AllowAnyMethod()
                    .AllowAnyHeader();
        });
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
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

app.UseCors("AllowSpecificOrigins");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
