using Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer;
using Activator.DomainDrivenDesigner.Support.Core.Middleware;
using Common.Core.Authentication;
using Common.Core.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Add Serilog Support
builder.Services.AddSerilog(
    (configure) =>
        configure.ReadFrom.Configuration(builder.Configuration));

// Add Authentication
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtCusScheme(builder.Configuration.GetSection("JWT").Get<JWTOptions>()!);

// Add Local Cache Support
builder.Services.AddMemoryCache();

// Add Local Redis Cache Support
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = builder.Configuration["Application:Properties:Name"];
});

// Add JWT Options
builder.Services.Configure<JWTOptions>(builder.Configuration.GetSection("JWT"));

// Add Database
var connectionString = builder.Configuration.GetConnectionString("DomainDrivenDesignerDatabase");
builder.Services.AddDatabase(connectionString);

// Register Services
builder.Services
    .RegisterDomain(
    "Activator.DomainDrivenDesigner.Server",
    "Activator.DomainDrivenDesigner.Application", 
    "Activator.DomainDrivenDesigner.Domain", 
    "Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer", 
    "Activator.DomainDrivenDesigner.Support.Core");

// Add CORS to allow cross domain query
builder.Services.AddCors(option =>
{
    option.AddPolicy("AllowDDDClientPolicy", builder => builder.AllowAnyOrigin().AllowAnyHeader());
});

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapStaticAssets();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("AllowDDDClientPolicy");

// Generate TraceId
app.UseMiddleware<RequestArrivalMiddleware>();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
