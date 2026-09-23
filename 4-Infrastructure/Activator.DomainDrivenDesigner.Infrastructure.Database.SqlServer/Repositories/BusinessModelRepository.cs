using Activator.DomainDrivenDesigner.Domain.BusinessModel;
using Activator.DomainDrivenDesigner.Domain.BusinessModel.Entities;
using Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Context;
using Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Entities;
using Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Exceptions;
using Common.Core.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Repositories
{
    [ServiceLocate(typeof(IBusinessModelRepository))]
    public class BusinessModelRepository : IBusinessModelRepository
    {
        private readonly DomainDbContext _dbContext;

        public BusinessModelRepository(DomainDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BusinessModel> RetrieveBusinessModelById(Guid businessModelId)
        {
            var rowBusinessModel = await LoadByIdFromDb(businessModelId).ConfigureAwait(false);
            return MapToBusniessModelDoaminModel(rowBusinessModel);
        }

        public async Task<List<BusinessModel>> RetrieveBusinessModelsByRequirementId(Guid requirementId)
        {
            var rowsBusinessModel = await LoadByRequirementIdFromDb(requirementId).ConfigureAwait(false);
            if (!rowsBusinessModel.Any())
            {
                throw new DomainEntityNotFoundException($"No Business Models found for Requirement ID({requirementId}).");
            }

            return rowsBusinessModel.Select(MapToBusniessModelDoaminModel).ToList();
        }

        private async Task<T_BUSINESS_MODEL> LoadByIdFromDb(Guid businessModelId)
        {
            var rowBusinessModel = await _dbContext.T_BUSINESS_MODELs
                .Include(bm => bm.CONTEXT)
                .FirstOrDefaultAsync(bm => bm.ID == businessModelId, default).ConfigureAwait(false);

            DomainEntityNotFoundException.ThrowIfNull<T_BUSINESS_MODEL>(businessModelId, rowBusinessModel);
            return rowBusinessModel;
        }

        private async Task<List<T_BUSINESS_MODEL>> LoadByRequirementIdFromDb(Guid requirementId)
        {
            var rowsBusinessModel = await _dbContext.T_BUSINESS_MODELs
                .Include(bm => bm.CONTEXT)
                .Where(bm => bm.REQUIREMENT_ID == requirementId)
                .ToListAsync(default).ConfigureAwait(false);

            return rowsBusinessModel;
        }

        private BusinessModel MapToBusniessModelDoaminModel(T_BUSINESS_MODEL rowBusinessModel)
        {
            return new BusinessModel(rowBusinessModel.ID)
            {
                Name = rowBusinessModel.NAME,
                ContentMermaid = rowBusinessModel.RAW_DESCRIPTION,
                ContextId = rowBusinessModel.CONTEXT_ID
            };
        }
    }
}