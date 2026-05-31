using Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer;
using Activator.DomainDrivenDesigner.Support.Core.Middleware;
using Common.Core.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Add Database
var connectionString = builder.Configuration.GetConnectionString("DomainDrivenDesignerDatabase");
builder.Services.AddDatabase(connectionString);

// Register Services
builder.Services
    .RegisterDomain("Activator.DomainDrivenDesigner.Server", "Activator.DomainDrivenDesigner.Application", "Activator.DomainDrivenDesigner.Domain", "Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer", "Activator.DomainDrivenDesigner.Support.Core");

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
