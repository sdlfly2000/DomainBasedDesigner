using Activator.DomainDrivenDesigner.Domain.Entities;
using Activator.DomainDrivenDesigner.Domain.Repositories;
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
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<Guid?> CreateProject(Project project)
    {
        if (project == null) throw new ArgumentNullException(nameof(project));

        var newProject = new T_PROJECT
        {
            ID = Guid.NewGuid(),
            NAME = project.Name,
            DESCRIPTION = project.Description,
            CREATED_UTC = DateTime.UtcNow
        };

        _dbContext.T_PROJECTs.Add(newProject);
        await _dbContext.SaveChangesAsync().ConfigureAwait(false);

        return newProject.ID;
    }

    public async Task<List<Project>> RetrieveFullProjects()
    {
        var loadedProjects = await _dbContext.T_PROJECTs.Include(p => p.T_REQUIREMENTs).ToListAsync().ConfigureAwait(false);

        var projects = new List<Project>();

        foreach (var rowProject in loadedProjects)
        {
            var project = Map(rowProject);

            project.Requirements = rowProject.T_REQUIREMENTs.Select(Map).ToList();

            projects.Add(project);
        }

        return projects;
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
}