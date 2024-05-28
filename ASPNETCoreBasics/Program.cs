using ASPNETCoreBasics.Configurations;
using ASPNETCoreBasics.Contexts;
using ASPNETCoreBasics.Contexts.ASPNETCoreBasics.Models;
using ASPNETCoreBasics.Middleware;
using ASPNETCoreBasics.Profiles;
using ASPNETCoreBasics.Repository;
using ASPNETCoreBasics.Services;
using ASPNETCoreBasics.Validator;
using ASPNETCoreBasics.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Configuration;
using System.Data.Entity;
/*
var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

var configuration = builder.Configuration;
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Services.AddEndpointsApiExplorer();
var connectionString = configuration.GetConnectionString("SQLBD");
builder.Services.AddDbContext<WeatherForecastDbContext>(options =>
    options.UseSqlServer(connectionString), ServiceLifetime.Scoped);
builder.Services.AddDbContext<UserContext>(options =>
    options.UseSqlServer(connectionString), ServiceLifetime.Scoped);

//builder.Services.AddValidatorsFromAssemblyContaining<WeatherForecastRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<TestValidator>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IWeatherForecastRepository, WeatherForecastRepository>();
builder.Services.AddScoped<IWeatherForecastService, WeatherForecastService>();
builder.Services.AddScoped<MyService>();
builder.Services.AddAutoMapper(typeof(MappingProfile));
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseMiddleware<TimeMiddleware>();

app.MapControllers();
app.Run();*/

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

var configuration = builder.Configuration;
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
var connectionString = configuration.GetConnectionString("SQLBD");
builder.Services.AddDbContext<WeatherForecastDbContext>(options =>
    options.UseSqlServer(connectionString), ServiceLifetime.Scoped);
builder.Services.AddDbContext<UserContext>(options =>
    options.UseSqlServer(connectionString), ServiceLifetime.Scoped);

builder.Services.AddValidatorsFromAssemblyContaining<TestValidator>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IWeatherForecastRepository, WeatherForecastRepository>();
builder.Services.AddScoped<IWeatherForecastService, WeatherForecastService>();
builder.Services.AddScoped<MyService>();
builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseMiddleware<TimeMiddleware>();

app.MapControllers();
app.Run();