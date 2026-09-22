using Activator.DomainDrivenDesigner.Domain.Project;
using Activator.DomainDrivenDesigner.Domain.Project.Entities;
using Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Context;
using Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Entities;
using Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Exceptions;
using Common.Core.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Repositories
{
    [ServiceLocate(typeof(IProjectRepository))]
    public class ProjectRepository : IProjectRepository
    {
        private readonly DomainDbContext _dbContext;

        public ProjectRepository(DomainDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Project>> RetrieveFullProjects()
        {
            var projectsDbEntities = await _dbContext.T_PROJECTs
                .Include(p => p.T_REQUIREMENTs)
                .ToListAsync().ConfigureAwait(false);

            var projects = projectsDbEntities.Select(MapToProjectDomainModel).ToList();
            return projects;
        }

        public async Task<Project> RetrieveProjectById(Guid projectId)
        {
            var projectDbEntity = await _dbContext.T_PROJECTs
                .Include(p => p.T_REQUIREMENTs)
                .FirstOrDefaultAsync(p => p.ID == projectId).ConfigureAwait(false);

            DomainEntityNotFoundException.ThrowIfNull(projectId, projectDbEntity);
            return MapToProjectDomainModel(projectDbEntity);
        }

        private Project MapToProjectDomainModel(T_PROJECT rowProject)
        {
            var project = new Project(rowProject.ID, rowProject.NAME)
            {
                Description = rowProject.DESCRIPTION,
                CreatedOnUtc = rowProject.CREATED_UTC
            };

            foreach (var requirementDbEntity in rowProject.T_REQUIREMENTs)
            {
                project.Requirements.Add(MapToRequirementDomainModel(requirementDbEntity));
            }

            return project;
        }

        private Requirement MapToRequirementDomainModel(T_REQUIREMENT rowRequirment)
        {
            var requirement = new Requirement(rowRequirment.ID)
            {
                Description = rowRequirment.DESCRIPTION,
                CreatedOnUtc = rowRequirment.CREATE_UTC
            };

            return requirement;
        }
    }
}