using Activator.DomainDrivenDesigner.Domain.Repositories;
using Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Context;
using Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Entities;
using Common.Core.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Repositories;

[ServiceLocate(typeof(IContextRepository))]
public class ContextRepository : IContextRepository
{
    private readonly DomainDbContext _domainDbContext;

    public ContextRepository(DomainDbContext domainDbContext)
    {
        _domainDbContext = domainDbContext;
    }

    public async Task<List<Domain.Entities.Context>> RetrieveContexts(Guid projectId)
    {
        var contexts = await _domainDbContext.T_BUSINESS_CONTEXTs
            .Where(x => x.T_PROJECT_ID == projectId)
            .ToListAsync()
            .ConfigureAwait(false);

        return contexts.Select(Map).ToList();
    }

    public async Task<Guid> CreateContext(string name, Guid projectId)
    {
        var rowBusinessContext = new T_BUSINESS_CONTEXT
        {
            ID = Guid.NewGuid(),
            NAME = name,
            CREATED_UTC = DateTime.UtcNow,
            T_PROJECT_ID = projectId
        };

        _domainDbContext.T_BUSINESS_CONTEXTs.Add(rowBusinessContext);

        await _domainDbContext.SaveChangesAsync()
            .ConfigureAwait(false);

        return rowBusinessContext.ID;
    }

    private Domain.Entities.Context Map(T_BUSINESS_CONTEXT rowBusinessContext)
    {
        var context = new Domain.Entities.Context(rowBusinessContext.ID);
        context.Name = rowBusinessContext.NAME;
        context.CreatedOnUtc = rowBusinessContext.CREATED_UTC;
        return context;
    }
}