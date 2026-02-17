using AutoMapper;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using SetelaServerV3._1;
using SetelaServerV3._1.Application.Features.AssignmentFeature;
using SetelaServerV3._1.Application.Features.AssignmentSubmissionFeature;
using SetelaServerV3._1.Application.Features.Auth.Config;
using SetelaServerV3._1.Application.Features.CourseFeature;
using SetelaServerV3._1.Application.Features.ExamFeature;
using SetelaServerV3._1.Application.Features.ExamSubmissionFeature;
using SetelaServerV3._1.Application.Features.ModuleFeature;
using SetelaServerV3._1.Application.Features.TopicSeparatorFeature;
using SetelaServerV3._1.Application.Features.UserFeature;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Common;
using SetelaServerV3._1.Shared.Common.Interfaces;
using SetelaServerV3._1.Shared.Common.Services;
using SetelaServerV3._1.Shared.Policies;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


// Define a specific policy name
var allowedOrigins = "setela_client_v3.1";

// Add CORS services to the container
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowedOrigins,
        builder =>
        {
            // **CRITICAL:** Replace 'http://localhost:5173' with the exact URL of your frontend application.
            // You can add multiple origins separated by commas or by calling WithOrigins multiple times.
            builder.WithOrigins("http://localhost:5173")
                   // This allows all standard HTTP methods (GET, POST, PUT, DELETE, OPTIONS, etc.)
                   .AllowAnyMethod()
                   // This allows all headers to be sent in the request
                   .AllowAnyHeader()
                   // Optional but recommended for development: allows credentials (like cookies) to be sent
                   .AllowCredentials();
        });
});

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
// Program.cs
//builder.Services.AddDbContextFactory<AppDbContext>(options =>
    //options.UseNpgsql(connectionString));

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddScoped<IPermissionHandler, Permissions>();
builder.Services.AddScoped<MaxDisplayOrder>();
builder.Services.AddScoped<IFileStorage, LocalFileService>();

builder.Services.AddAuthorization();




builder.Services.AddAutoMapper(
    typeof(GeneralMappingProfile).Assembly,
    typeof(CourseMappingProfile).Assembly, 
    typeof(UserMappingProfile).Assembly,
    typeof(TopicSeparatorMappingProfile).Assembly,
    typeof(ModuleMappingProfile).Assembly,
    typeof(AssignmentSubmissionMappingProfile).Assembly,
    typeof(ExamMapingProfile).Assembly,
    typeof(ExamSubmissionMappingProfile).Assembly,
    typeof(AssignmentMappingProfile).Assembly);
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddExceptionHandler<ExceptionHandler>();

var app = builder.Build();

var uploadPath = builder.Configuration["UploadArea"];
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadPath),
    RequestPath = "/cdn"
});

app.UseExceptionHandler(options => { });


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(allowedOrigins);

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
