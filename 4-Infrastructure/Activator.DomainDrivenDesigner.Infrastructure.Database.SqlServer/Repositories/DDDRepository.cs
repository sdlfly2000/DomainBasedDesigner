using Activator.DomainDrivenDesigner.Domain.Entities;
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

    public async Task<Guid?> CreateProject(Project project)
    {
        var efProject = new T_PROJECT
        {
            ID = project.Id,
            NAME = project.Name,
            DESCRIPTION = project.Description,
            CREATED_UTC = project.CreatedOnUtc
        };

        _context.T_PROJECTs.Add(efProject);
        await _context.SaveChangesAsync().ConfigureAwait(false);

        return project.Id;
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

    public async Task<List<Project>> RetrieveFullProjects()
    {
        var efProjects = await _context.T_PROJECTs
            .Include(p => p.T_REQUIREMENTs)
            .ToListAsync()
            .ConfigureAwait(false);

        return [.. efProjects.Select(p => {
            var project = Map(p);
            var requirements = p.T_REQUIREMENTs.Select(r => Map(r)).ToList();
            project.Requirements.AddRange(requirements);
            return project;
        })];
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

    public async Task<List<BusinessModel>> RetrieveBusinessModelsByProjectId(Guid projectId)
    {
        var rowProject = await _context.T_PROJECTs
            .Include(p => p.T_REQUIREMENTs)
            .ThenInclude(r => r.T_BUSINESS_MODELs)
            .ThenInclude(bm => bm.CONTEXT)
            .SingleOrDefaultAsync(p => p.ID == projectId)
            .ConfigureAwait(false);

        DomainEntityNotFoundException.ThrowIfNull(projectId, rowProject);

        var businessModels = new List<BusinessModel>();

        var rowBusinessModels = rowProject.T_REQUIREMENTs.SelectMany(r => r.T_BUSINESS_MODELs);
        businessModels.AddRange(rowBusinessModels.Select(bm => Map(bm)));

        return businessModels;
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

    public async Task<BusinessModel> RetrieveBusinessModelsById(Guid businessModelId)
    {
        var rowBusinessModel = await _context.T_BUSINESS_MODELs
            .Include(bm => bm.CONTEXT)
            .SingleOrDefaultAsync(bm => bm.ID == businessModelId)
            .ConfigureAwait(false);

        DomainEntityNotFoundException.ThrowIfNull(businessModelId, rowBusinessModel);

        return Map(rowBusinessModel);
    }

    public async Task<Guid> UpdateBusinessModels(BusinessModel model)
    {
        var rowBusinessModel = await _context
            .T_BUSINESS_MODELs
            .SingleOrDefaultAsync(bm => bm.ID == model.Id)
            .ConfigureAwait(false);

        DomainEntityNotFoundException.ThrowIfNull(model.Id, rowBusinessModel);

        Persist(model, rowBusinessModel);
        
        await _context.SaveChangesAsync().ConfigureAwait(false);

        return model.Id;
    }

    public async Task<List<Domain.Entities.Context>> RetrieveContexts()
    {
        return await _context.T_BUSINESS_CONTEXTs
            .Select(c => Map(c))
            .ToListAsync();
    }

    public async Task<Guid> CreateContext(string name)
    {
        var contextEntry = _context.T_BUSINESS_CONTEXTs.Add(new T_BUSINESS_CONTEXT
        {
            ID = Guid.NewGuid(),
            NAME = name,
            CREATED_UTC = DateTime.UtcNow
        });

        await _context.SaveChangesAsync().ConfigureAwait(false);

        return contextEntry.Entity.ID;
    }

    #region Private Mapper

    private void Persist(BusinessModel model, T_BUSINESS_MODEL row)
    {
        row.NAME = model.Name;
        row.RAW_DESCRIPTION = model.ContentMermaid;
    }

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

    private Domain.Entities.Context Map(T_BUSINESS_CONTEXT rowBusinessContext)
    {
        return new Domain.Entities.Context(rowBusinessContext.ID)
        {
            Name = rowBusinessContext.NAME,
            CreatedOnUtc = rowBusinessContext.CREATED_UTC
        };
    }

    #endregion
}
