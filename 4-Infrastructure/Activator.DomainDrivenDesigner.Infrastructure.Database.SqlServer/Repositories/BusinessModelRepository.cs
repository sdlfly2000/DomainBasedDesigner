using Activator.DomainDrivenDesigner.Domain.BusinessModel;
using Activator.DomainDrivenDesigner.Domain.BusinessModel.Entities;
using Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Context;
using Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Entities;
using Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Exceptions;
using Common.Core.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Repositories;

[ServiceLocate(typeof(IBusinessModelRepository))]
public class BusinessModelRepository : IBusinessModelRepository
{
    private readonly DomainDbContext _dbContext;

    public BusinessModelRepository(DomainDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<BusinessModel> RetrieveBusinessModelsById(Guid businessModelId)
    {
        var rowBusinessModel = await LoadBusinessModelFromDb(businessModelId).ConfigureAwait(false);
        return Map(rowBusinessModel);
    }

    public async Task<Guid> UpdateBusinessModels(BusinessModel model)
    {
        var rowBusinessModel = await LoadBusinessModelFromDb(model.Id).ConfigureAwait(false);
        PersistDoaminModelToDbEntity(model, rowBusinessModel);
        await _dbContext.SaveChangesAsync().ConfigureAwait(false);
        return model.Id;
    }

    private async Task<T_BUSINESS_MODEL> LoadBusinessModelFromDb(Guid businessModelId)
    {
        var rowBusinessModel = await _dbContext.T_BUSINESS_MODELs
            .Include(b => b.CONTEXT)
            .SingleOrDefaultAsync(b => b.ID == businessModelId)
            .ConfigureAwait(false);

        DomainEntityNotFoundException.ThrowIfNull(businessModelId, rowBusinessModel);
        return rowBusinessModel;
    }

    private BusinessModel Map(T_BUSINESS_MODEL rowBusinessModel)
    {
        return new BusinessModel(rowBusinessModel.ID)
        {
            Name = rowBusinessModel.NAME,
            ContentMermaid = rowBusinessModel.RAW_DESCRIPTION,
            ContextId = rowBusinessModel.CONTEXT_ID
        };
    }

    private void PersistDoaminModelToDbEntity(BusinessModel model, T_BUSINESS_MODEL rowBusinessModel)
    {
        rowBusinessModel.NAME = model.Name;
        rowBusinessModel.RAW_DESCRIPTION = model.ContentMermaid;
        rowBusinessModel.CONTEXT_ID = model.ContextId;
    }
}