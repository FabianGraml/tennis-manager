using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Reflection;
using System.Text;
using Tennis.Database.Context;
using Tennis.Repository.UnitOfWork;
using Tennis.Service.AppSettingsService;
using Tennis.Service.AppSettingsService.AppSettings;
using Tennis.Service.AuthService;
using Tennis.Service.BookingService;

string corsKey = "_mySecretCorsKey";
var builder = WebApplication.CreateBuilder(args);
var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
builder.Services.Configure<AppSettingsConfig>(builder.Configuration.GetSection("Settings"));

// Add services to the container
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAppSettingsService<AppSettingsConfig>, AppSettingsService<AppSettingsConfig>>();

// Pre defined services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    x.SwaggerDoc("v1", new OpenApiInfo { Title = "TestSwagger", Version = "v1" });
});

// Configure appsettings.json DbContext
string baseDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
AppDomain.CurrentDomain.SetData("DataDirectory", baseDirectory);
string connectionString = configuration.GetConnectionString("TennisDb")!
    .Replace("|DataDirectory|", AppDomain.CurrentDomain
    .GetData("DataDirectory")!
    .ToString());

// Db Context here
builder.Services.AddDbContext<TennisContext>(options =>
{
    options.UseSqlServer(connectionString);
});

// Jwt stuff
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            builder.Configuration.GetSection("Settings").GetValue<string>("JwtSecretKey")
            ))
    };
});

// Cors configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy(corsKey,
        x => x.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader()
              );
});

var app = builder.Build();

// Migrate Database
// Apply Migrations using this command
// Add-Migration [Name] -StartupProject Tennis.Api -Context TennisContext -Project Tennis.Database
var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<TennisContext>();
context.Database.Migrate();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(corsKey);

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();