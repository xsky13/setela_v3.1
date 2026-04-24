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
using System.Collections.Concurrent;
using System.Security.Claims;
using System.Text;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);


var allowedOrigins = "setela_client_v3.1";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowedOrigins,
        builder =>
        {
            // **CRITICAL:** Replace 'http://localhost:5173' with the exact URL of your frontend application.
            builder.WithOrigins("http://localhost:5173")
                   .AllowAnyMethod()
                   .AllowAnyHeader()
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

string finalConn;
if (connectionString.Contains("://"))
{
    var uri = new Uri(connectionString);
    var userInfo = uri.UserInfo.Split(':');

    finalConn = $"Host={uri.Host};" +
                $"Port={uri.Port};" +
                $"Database={uri.AbsolutePath.TrimStart('/')};" +
                $"Username={userInfo[0]};" +
                $"Password={userInfo[1]};" +
                $"SSL Mode=Require;" +
                $"Trust Server Certificate=true;" +
                $"Pooling=true;" +
                $"Include Error Detail=true;";
}
else
{
    finalConn = connectionString;
}
// Program.cs
//builder.Services.AddDbContextFactory<AppDbContext>(options =>
//options.UseNpgsql(connectionString));

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(finalConn, npgsqlOptions =>
{
    npgsqlOptions.CommandTimeout(60);
}));

builder.Services.AddScoped<IPermissionHandler, Permissions>();
builder.Services.AddScoped<MaxDisplayOrder>();
//builder.Services.AddScoped<IFileStorage, LocalFileService>();
builder.Services.AddScoped<IFileStorage, SupabaseFileService>();
builder.Services.AddScoped<IResourceCleanupService, ResourceCleanupService>();

var supabaseUrl = Environment.GetEnvironmentVariable("SUPABASE_URL");
var supabaseKey = Environment.GetEnvironmentVariable("SUPABASE_KEY");
builder.Services.AddScoped(_ => new Supabase.Client(supabaseUrl, supabaseKey));

builder.Services.AddAuthorization();

builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
    {
        // if login, only 5 attempts per minute.
        // else 30 requests per minute
        string partitionKey = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)
                          ?? httpContext.Connection.RemoteIpAddress?.ToString()
                          ?? "anonymous";

        bool isAuthRoute = httpContext.Request.Path.Value?.Contains("/api/auth") ?? false;
        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: $"{partitionKey}_{isAuthRoute}",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = isAuthRoute ? 10 : 30,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0
        });
    });
});


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

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 100 * 1024 * 1024; // 100MB
});

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

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.UseHttpsRedirection();

app.UseCors(allowedOrigins);

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
