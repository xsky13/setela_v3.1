using AutoMapper;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SetelaServerV3._1;
using SetelaServerV3._1.Application.Features.Auth.Config;
using SetelaServerV3._1.Application.Features.CourseFeature;
using SetelaServerV3._1.Application.Features.TopicSeparatorFeature;
using SetelaServerV3._1.Application.Features.UserFeature;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Common;
using SetelaServerV3._1.Shared.Policies;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
Env.Load();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly));
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).AddEnvironmentVariables();
builder.Services.Configure<AuthOptions>(builder.Configuration.GetSection("Jwt"));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],

 		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("Jwt__Key") ?? throw new InvalidOperationException("No hay jwtkey"))),
        ValidIssuers = [builder.Configuration["Jwt:Issuer"]],
        ValidAudiences = [builder.Configuration["Jwt:Audience"]],
    };
});

var connectionString = Environment.GetEnvironmentVariable("DB_CONN");
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddScoped<IPermissionHandler, Permissions>();

builder.Services.AddAuthorization();




builder.Services.AddAutoMapper(
    typeof(GeneralMappingProfile).Assembly,
    typeof(CourseMappingProfile).Assembly, 
    typeof(UserMappingProfile).Assembly,
    typeof(TopicSeparatorMappingProfile).Assembly);
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddExceptionHandler<ExceptionHandler>();

var app = builder.Build();
app.UseExceptionHandler(options => { });


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
