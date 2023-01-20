using Microsoft.EntityFrameworkCore;
using Serilog;
using WebVilla.Data;
using WebVilla.Logging;

var builder = WebApplication.CreateBuilder(args);

string connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationContext>(opt => opt.UseNpgsql(connection));
// Add services to the container.
Log.Logger = new LoggerConfiguration()
    .MinimumLevel
    .Debug()
    .WriteTo.File("log/villaLogs.txt", rollingInterval:RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers(options =>
{
    options.ReturnHttpNotAcceptable = true;
    options.RespectBrowserAcceptHeader = true;
}).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();//.AddXmlSerializerFormatters();

builder.Services.AddSingleton<ILogging,Logging>();

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
