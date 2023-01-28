using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;
using Tennis.Database.Context;
using Tennis.Repository.UnitOfWork;
using Tennis.Service.BookingService;
using Tennis.Service.PersonService;

string corsKey = "_mySecretCorsKey";
var builder = WebApplication.CreateBuilder(args);
var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

// Add services to the container
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IPersonService, PersonService>();
//builder.Services.AddScoped<IBookingService, BookingService>();

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