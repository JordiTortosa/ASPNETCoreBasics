using ASPNETCoreBasics.Configurations;
using ASPNETCoreBasics.Contexts.ASPNETCoreBasics.Models;
using ASPNETCoreBasics.Middleware;
using ASPNETCoreBasics.Validator;
using ASPNETCoreBasics.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Data.Entity;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Services.AddEndpointsApiExplorer();
var connectionString = configuration.GetConnectionString("SQLBD");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

//builder.Services.AddValidatorsFromAssemblyContaining<WeatherForecastRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<TestValidator>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<MyService>();
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