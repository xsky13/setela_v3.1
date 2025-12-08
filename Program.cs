using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Infrastructure.Data;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
Env.Load();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly));


var connectionString = Environment.GetEnvironmentVariable("DB_CONN");
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
