using Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer;

public static class DatabaseConfiguration
{
    public static IServiceCollection AddDatabase(this IServiceCollection serviceCollection, string? connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
        {
            return serviceCollection;
        }
        
        return serviceCollection.AddDbContextPool<DomainDbContext>(
            options => options.UseSqlServer(
                connectionString)
        );
    }
}
