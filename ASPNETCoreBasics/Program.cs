using ASPNETCoreBasics.Configurations;
using ASPNETCoreBasics.Middleware;
using ASPNETCoreBasics.Validator;
using ASPNETCoreBasics.Validators;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddValidatorsFromAssemblyContaining<WeatherForecastRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<TestValidator>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<MyService>();
var configuration = builder.Configuration;
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

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