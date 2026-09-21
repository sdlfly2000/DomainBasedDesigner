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

        public async Task<Guid?> CreateProject(Project project)
        {
            var newProject = Persist(project);
            await _dbContext.T_PROJECTs.AddAsync(newProject).ConfigureAwait(false);
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            return newProject.ID;
        }

        public async Task<List<Project>> RetrieveFullProjects()
        {
            var projectsDbEntities = await _dbContext.T_PROJECTs
                .Include(p => p.T_REQUIREMENTs)
                .ToListAsync().ConfigureAwait(false);

            var projects = projectsDbEntities.Select(Map).ToList();
            return projects;
        }

        public async Task<Project> RetrieveProjectById(Guid projectId)
        {
            var projectDbEntity = await _dbContext.T_PROJECTs
                .Include(p => p.T_REQUIREMENTs)
                .FirstOrDefaultAsync(p => p.ID == projectId).ConfigureAwait(false);

            DomainEntityNotFoundException.ThrowIfNull(projectId, projectDbEntity);
            return Map(projectDbEntity);
        }

        public async Task<Guid?> CreateRequirement(Requirement requirement, Guid projectId)
        {
            var newRequirementDbEntity = Persist(requirement, projectId);
            await _dbContext.T_REQUIREMENTs.AddAsync(newRequirementDbEntity).ConfigureAwait(false);
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            return newRequirementDbEntity.ID;
        }

        public async Task<Guid?> UpdateRequirement(Requirement requirement)
        {
            var loadRequirementDbEntity = await _dbContext.T_REQUIREMENTs
                .FirstOrDefaultAsync(r => r.ID == requirement.ID).ConfigureAwait(false);

            DomainEntityNotFoundException.ThrowIfNull(requirement.ID, loadRequirementDbEntity);

            loadRequirementDbEntity.DESCRIPTION = requirement.Description;
            loadRequirementDbEntity.CREATE_UTC = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            return requirement.ID;
        }

        private T_PROJECT Persist(Project project)
        {
            var tProject = new T_PROJECT
            {
                ID = project.ID,
                NAME = project.Name,
                DESCRIPTION = project.Description,
                CREATED_UTC = DateTime.UtcNow
            };

            foreach (var requirement in project.Requirements)
            {
                tProject.T_REQUIREMENTs.Add(Persist(requirement, project.ID));
            }

            return tProject;
        }

        private T_REQUIREMENT Persist(Requirement requirement, Guid projectId)
        {
            var tRequirement = new T_REQUIREMENT
            {
                ID = requirement.ID,
                DESCRIPTION = requirement.Description,
                CREATE_UTC = DateTime.UtcNow,
                PROJECT_ID = projectId
            };

            return tRequirement;
        }

        private Project Map(T_PROJECT rowProject)
        {
            var project = new Project(rowProject.ID, rowProject.NAME)
            {
                Description = rowProject.DESCRIPTION,
                CreatedOnUtc = rowProject.CREATED_UTC
            };

            foreach (var requirementDbEntity in rowProject.T_REQUIREMENTs)
            {
                project.Requirements.Add(Map(requirementDbEntity));
            }

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
}