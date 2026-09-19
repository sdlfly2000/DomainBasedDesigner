using Activator.DomainDrivenDesigner.Domain.BusinessModel.Entities;
using Activator.DomainDrivenDesigner.Domain.Project.Entities;
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

    public async Task<Guid?> CreateRequirement(Requirement requirement, Guid projectId)
    {
        var rowRequirement = new T_REQUIREMENT
        {
            ID = requirement.Id,
            DESCRIPTION = requirement.Description,
            PROJECT_ID = projectId,
            CREATE_UTC = DateTime.UtcNow
        };
         _context.T_REQUIREMENTs.Add(rowRequirement);
        await _context.SaveChangesAsync().ConfigureAwait(false);

        return requirement.Id;
    }

    public async Task<Guid?> UpdateRequirement(Requirement requirement)
    {
        var rowRequirement = await _context.T_REQUIREMENTs
            .SingleOrDefaultAsync(r => r.ID == requirement.Id)
            .ConfigureAwait(false);

        DomainEntityNotFoundException.ThrowIfNull(requirement.Id, rowRequirement);

        rowRequirement.DESCRIPTION = requirement.Description;

        _context.T_REQUIREMENTs.Update(rowRequirement);

        await _context.SaveChangesAsync().ConfigureAwait(false);

        return requirement.Id;
    }

    public async Task<Project> RetrieveProjectById(Guid projectId)
    {
        var rowProject = await _context.T_PROJECTs
            .SingleOrDefaultAsync(p => p.ID == projectId)
            .ConfigureAwait(false);

        DomainEntityNotFoundException.ThrowIfNull(projectId, rowProject);

        return Map(rowProject);
    }

    public async Task<List<Requirement>> RetrieveRequirementByProjectId(Guid projectId)
    {
        var rowProject = await _context.T_PROJECTs
            .Include(p => p.T_REQUIREMENTs)
            .SingleOrDefaultAsync(p => p.ID == projectId)
            .ConfigureAwait(false);

        DomainEntityNotFoundException.ThrowIfNull(projectId, rowProject);

        return [.. rowProject
            .T_REQUIREMENTs
            .Select(r => Map(r))];
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

    public async Task<Requirement> RetrieveRequirementById(Guid requirementId)
    {
        var rowRequirement = await _context.T_REQUIREMENTs
            .Include(r => r.T_BUSINESS_MODELs)
            .SingleOrDefaultAsync(r => r.ID == requirementId)
            .ConfigureAwait(false);

        DomainEntityNotFoundException.ThrowIfNull(requirementId, rowRequirement);

        var requirement = Map(rowRequirement);
        var businessModels = rowRequirement.T_BUSINESS_MODELs.Select(bm => Map(bm)).ToList();
        requirement.BusinessModels.AddRange(businessModels);

        return requirement;
    }

    #region Private Mapper

    private Project Map(T_PROJECT rowProject)
    {
        var project = new Project(rowProject.ID, rowProject.NAME)
        {
            Description = rowProject.DESCRIPTION,
            CreatedOnUtc = rowProject.CREATED_UTC
        };
        return project;
    }

    private Requirement Map(T_REQUIREMENT rowRequirment)
    {
        var requirement = new Requirement(rowRequirment.ID)
        {
            Description = rowRequirment.DESCRIPTION,
            CreatedOnUtc = rowRequirment.CREATE_UTC
        };

        return requirement;
    }

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
