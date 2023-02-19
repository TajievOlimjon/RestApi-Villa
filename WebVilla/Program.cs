using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using WebVilla.AuthModels;
using WebVilla.Data;
using WebVilla.ExtensionMethods.AddDIService;
using WebVilla.ExtensionMethods.AuthServices;
using WebVilla.ExtensionMethods.SwaggerServices;

var builder = WebApplication.CreateBuilder(args);
// Add db configure
string connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationContext>(options => {
    options.UseNpgsql(connection);
 });
//Add auth configure
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(config =>
{
    config.Password.RequiredLength = 4;
    config.Password.RequireDigit = false;
    config.Password.RequireLowercase = false;
    config.Password.RequireNonAlphanumeric = false;
    config.Password.RequireUppercase = false;
}).AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();
//add cache service
builder.Services.AddResponseCaching();
// Add log configure.
Log.Logger = new LoggerConfiguration()
    .MinimumLevel
    .Debug()
    .WriteTo.File("log/villaLogs.txt", rollingInterval:RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();
// Add 
builder.Services.AddControllers(options =>
{
    options.CacheProfiles.Add("Default30",
        new CacheProfile
        {
            Duration = 30
        });
    options.ReturnHttpNotAcceptable = false;
    options.RespectBrowserAcceptHeader = true;
}).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();//.AddXmlSerializerFormatters();

// Add services to the container
builder.Services.AddServicesToLifeTimeDI();

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1,0);
    options.ReportApiVersions = true;
});

builder.Services.AddVersionedApiExplorer(x =>
{
    x.GroupNameFormat = "'v'VVV";
    x.SubstituteApiVersionInUrl = true;
});

//add Auth  configuration 
builder.Services.AuthService(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//add swaggergen configuration 

builder.Services.SwaggerService();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json","VillaV1");
        options.SwaggerEndpoint("/swagger/v2/swagger.json", "VillaV2");
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
