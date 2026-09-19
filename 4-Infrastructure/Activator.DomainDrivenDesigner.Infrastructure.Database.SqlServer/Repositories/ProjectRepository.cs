using Activator.DomainDrivenDesigner.Domain.Project;
using Activator.DomainDrivenDesigner.Domain.Project.Entities;
using Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Context;
using Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Entities;
using Common.Core.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Repositories;

[ServiceLocate(typeof(IProjectRepository))]
public class ProjectRepository : IProjectRepository
{
    private readonly DomainDbContext _dbContext;

    public ProjectRepository(DomainDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid?> CreateProject(Project project)
    {
        var newProject = Persist(project);
        await _dbContext.SaveChangesAsync().ConfigureAwait(false);
        return newProject.ID;
    }

    public async Task<List<Project>> RetrieveFullProjects()
    {
        var rows = await _dbContext.T_PROJECTs
            .Include(p => p.T_REQUIREMENTs)
            .ToListAsync().ConfigureAwait(false);

        return rows.Select(Map).ToList();
    }

    private T_PROJECT Persist(Project project)
    {
        var rowProject = new T_PROJECT
        {
            ID = project.Id,
            NAME = project.Name,
            DESCRIPTION = project.Description,
            CREATED_UTC = DateTime.UtcNow
        };

        _dbContext.T_PROJECTs.Add(rowProject);
        return rowProject;
    }

    private Project Map(T_PROJECT rowProject)
    {
        var project = new Project(rowProject.ID, rowProject.NAME)
        {
            Description = rowProject.DESCRIPTION,
            CreatedOnUtc = rowProject.CREATED_UTC
        };

        foreach (var requirementRow in rowProject.T_REQUIREMENTs)
        {
            project.Requirements.Add(Map(requirementRow));
        }

        return project;
    }

    private Requirement Map(T_REQUIREMENT rowRequirment)
    {
        return new Requirement(rowRequirment.ID)
        {
            Description = rowRequirment.DESCRIPTION,
            CreatedOnUtc = rowRequirment.CREATE_UTC
        };
    }
}