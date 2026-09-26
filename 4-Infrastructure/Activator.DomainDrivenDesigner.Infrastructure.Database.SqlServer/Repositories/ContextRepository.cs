using Activator.DomainDrivenDesigner.Domain.Context;
using Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Context;
using Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Entities;
using Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Exceptions;
using Common.Core.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Repositories;

[ServiceLocate(typeof(IContextRepository))]
public class ContextRepository : IContextRepository
{
    private readonly DomainDbContext _dbContext;

    public ContextRepository(DomainDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Domain.Context.Entities.Context>> RetrieveContexts(Guid projectId)
    {
        var loadAllContextDatabaseEntity = await _dbContext.T_BUSINESS_CONTEXTs
            .Where(context => context.T_PROJECT_ID == projectId)
            .ToListAsync()
            .ConfigureAwait(false);

        var mapToContext = loadAllContextDatabaseEntity.Select(rowContext => this.mapToContext(rowContext)).ToList();

        return mapToContext;
    }

    public async Task<Guid> CreateContext(string name, Guid projectId)
    {
        var newContext = new T_BUSINESS_CONTEXT
        {
            ID = Guid.NewGuid(),
            NAME = name,
            CREATED_UTC = DateTime.UtcNow,
            T_PROJECT_ID = projectId
        };

        _dbContext.T_BUSINESS_CONTEXTs.Add(newContext);
        await _dbContext.SaveChangesAsync().ConfigureAwait(false);

        return newContext.ID;
    }

    public async Task<Guid> UpdateContext(Domain.Context.Entities.Context context)
    {
        var loadContextDatabseEntity = await _dbContext.T_BUSINESS_CONTEXTs.FindAsync(context.ID).ConfigureAwait(false);

        DomainEntityNotFoundException.ThrowIfNull(context.ID, loadContextDatabseEntity);

        loadContextDatabseEntity.NAME = context.Name;

        await _dbContext.SaveChangesAsync().ConfigureAwait(false);

        return loadContextDatabseEntity.ID;
    }

    private Domain.Context.Entities.Context mapToContext(T_BUSINESS_CONTEXT rowContext)
    {
        return new Domain.Context.Entities.Context(rowContext.ID)
        {
            Name = rowContext.NAME
        };
    }
}