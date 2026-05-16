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
        var efProject = new Entities.T_PROJECT
        {
            ID = project.Id,
            NAME = project.Name,
            CREATED_UTC = project.CreatedOnUTC
        };

        _context.T_PROJECTs.Add(efProject);
        await _context.SaveChangesAsync().ConfigureAwait(false);

        return project.Id;
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

    public async Task<List<BusinessModel>> RetrieveBusinessModelsByProjectId(Guid projectId)
    {
        var rowProject = await _context.T_PROJECTs
            .Include(p => p.T_REQUIREMENTs)
                .ThenInclude(r => r.T_BUSINESS_MODELs)
            .SingleOrDefaultAsync(p => p.ID == projectId)
            .ConfigureAwait(false);

        DomainEntityNotFoundException.ThrowIfNull(projectId, rowProject);

        var businessModels = new List<BusinessModel>();

        var rowBusinessModels = rowProject!.T_REQUIREMENTs.SelectMany(r => r.T_BUSINESS_MODELs);
        businessModels.AddRange(rowBusinessModels.Select(bm => Map(bm)));

        return businessModels;
    }

    public async Task<List<BusinessModel>> RetrieveBusinessModelsByRequirementId(Guid requirementId)
    {
        var rowRequirement = await _context.T_REQUIREMENTs
            .Include(r => r.T_BUSINESS_MODELs)
            .SingleOrDefaultAsync(r => r.ID == requirementId)
            .ConfigureAwait(false);

        DomainEntityNotFoundException.ThrowIfNull(requirementId, rowRequirement);
        

        return rowRequirement!
            .T_BUSINESS_MODELs
            .Select(bm => Map(bm))
            .ToList();
    }

    public async Task<BusinessModel> RetrieveBusinessModelsById(Guid businessModelId)
    {
        var rowBusinessModel = await _context.T_BUSINESS_MODELs
            .Include(bm => bm.T_BUSINESS_MODEL_PROPERTies)
            .SingleOrDefaultAsync(bm => bm.ID == businessModelId)
            .ConfigureAwait(false);

        DomainEntityNotFoundException.ThrowIfNull(businessModelId, rowBusinessModel);

        return Map(rowBusinessModel!);
    }

    #region Private Mapper

    private Project Map(T_PROJECT rowProject)
    {
        var project = new Project(rowProject.ID, rowProject.NAME);
        return project;
    }

    private Requirement Map(T_REQUIREMENT rowRequirment)
    {
        var requirement = new Requirement(rowRequirment.ID)
        {
            Description = rowRequirment.DESCRIPTION
        };

        return requirement;
    }

    private BusinessModel Map(T_BUSINESS_MODEL rowBusinessModel)
    {
        var businessModel = new BusinessModel(rowBusinessModel.ID)
        {
            Name = rowBusinessModel.NAME
        };

        return businessModel;
    }

    #endregion
}
