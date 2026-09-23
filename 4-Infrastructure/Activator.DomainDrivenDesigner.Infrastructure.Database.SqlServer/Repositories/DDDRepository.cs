using Activator.DomainDrivenDesigner.Domain.BusinessModel.Entities;
using Activator.DomainDrivenDesigner.Domain.Repositories;
using Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Context;
using Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Entities;
using Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Exceptions;
using Common.Core.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Repositories;

[ServiceLocate(typeof(IDDDRepository))]
public class DDDRepository : IDDDRepository
{
    private readonly DomainDbContext _context;

    public DDDRepository(DomainDbContext context)
    {
        _context = context;
    }

    public async Task<Guid?> CreateBusinessModel(BusinessModel model, Guid requirementId)
    {
        var rowBusinessModel = new T_BUSINESS_MODEL
        {
            ID = Guid.NewGuid(),
            NAME = model.Name,
            RAW_DESCRIPTION = model.ContentMermaid,
            REQUIREMENT_ID = requirementId,
            CONTEXT_ID = model.ContextId,
            CREATED_UTC = DateTime.UtcNow,
        };

        _context.T_BUSINESS_MODELs.Add(rowBusinessModel);
        await _context.SaveChangesAsync().ConfigureAwait(false);

        return rowBusinessModel.ID;
    }

    public async Task<List<BusinessModel>> RetrieveBusinessModelsByRequirementId(Guid requirementId)
    {
        var rowRequirement = await _context.T_REQUIREMENTs
            .Include(r => r.T_BUSINESS_MODELs)
            .ThenInclude (bm => bm.CONTEXT)
            .SingleOrDefaultAsync(r => r.ID == requirementId)
            .ConfigureAwait(false);

        DomainEntityNotFoundException.ThrowIfNull(requirementId, rowRequirement);
        
        return rowRequirement
            .T_BUSINESS_MODELs
            .Select(bm => Map(bm))
            .ToList();
    }

    #region Private Mapper

    private BusinessModel Map(T_BUSINESS_MODEL rowBusinessModel)
    {
        var businessModel = new BusinessModel(rowBusinessModel.ID)
        {
            Name = rowBusinessModel.NAME,
            ContentMermaid = rowBusinessModel.RAW_DESCRIPTION,
            ContextId = rowBusinessModel.CONTEXT_ID,
        };

        return businessModel;
    }

    #endregion
}
