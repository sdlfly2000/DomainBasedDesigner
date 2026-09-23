using Activator.DomainDrivenDesigner.Domain.BusinessModel.Entities;
using Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Context;
using Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Entities;
using Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Exceptions;
using Common.Core.DependencyInjection;

namespace Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Persistors
{
    [ServiceLocate(typeof(IBusinessModelPersistor))]
    public class BusinessModelPersistor : IBusinessModelPersistor
    {
        private readonly DomainDbContext _dbContext;

        public BusinessModelPersistor(DomainDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> CreateBusinessModel(BusinessModel model, Guid requirementId)
        {
            var businessModelEntity = new T_BUSINESS_MODEL
            {
                ID = model.ID,
                NAME = model.Name,
                REQUIREMENT_ID = requirementId,
                RAW_DESCRIPTION = model.ContentMermaid,
                CONTEXT_ID = model.ContextId,
                CREATED_UTC = DateTime.UtcNow
            };

            _dbContext.T_BUSINESS_MODELs.Add(businessModelEntity);
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

            return businessModelEntity.ID;
        }

        public async Task<Guid> UpdateBusinessModel(BusinessModel model)
        {
            var businessModelEntity = await _dbContext.T_BUSINESS_MODELs.FindAsync(model.ID).ConfigureAwait(false);
            DomainEntityNotFoundException.ThrowIfNull(model.ID, businessModelEntity);

            businessModelEntity.NAME = model.Name;
            businessModelEntity.REQUIREMENT_ID = model.ContextId;
            businessModelEntity.RAW_DESCRIPTION = model.ContentMermaid;

            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

            return businessModelEntity.ID;
        }
    }
}