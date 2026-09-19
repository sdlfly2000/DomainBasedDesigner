using Activator.DomainDrivenDesigner.Domain.Context;
using Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Context;
using Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Entities;
using Common.Core.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Repositories;

[ServiceLocate(typeof(IContextRepository))]
public class ContextRepository : IContextRepository
{
    private readonly DomainDbContext _domainDbCtx;

    public ContextRepository(DomainDbContext domainDbCtx)
    {
        _domainDbCtx = domainDbCtx;
    }

    public async Task<List<Domain.Context.Entities.Context>> RetrieveContexts(Guid projectId)
    {
        var contexts = await _domainDbCtx.T_BUSINESS_CONTEXTs
            .Where(x => x.T_PROJECT_ID == projectId)
            .ToListAsync().ConfigureAwait(false);
        return contexts.Select(Map).ToList();
    }

    public async Task<Guid> CreateContext(string name, Guid projectId)
    {
        var context = new T_BUSINESS_CONTEXT
        {
            ID = Guid.NewGuid(),
            NAME = name,
            CREATED_UTC = DateTime.UtcNow,
            T_PROJECT_ID = projectId
        };

        await _domainDbCtx.T_BUSINESS_CONTEXTs.AddAsync(context).ConfigureAwait(false);
        await _domainDbCtx.SaveChangesAsync().ConfigureAwait(false);
        return context.ID;
    }

    private Domain.Context.Entities.Context Map(T_BUSINESS_CONTEXT rowBusinessContext)
    {
        return new Domain.Context.Entities.Context(rowBusinessContext.ID)
        {
            Name = rowBusinessContext.NAME ?? string.Empty,
            CreatedOnUtc = rowBusinessContext.CREATED_UTC
        };
    }
}