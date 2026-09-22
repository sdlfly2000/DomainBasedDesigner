using Activator.DomainDrivenDesigner.Domain.Project;
using Activator.DomainDrivenDesigner.Domain.Project.Entities;
using Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Context;
using Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Entities;
using Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Exceptions;
using Common.Core.DependencyInjection;

namespace Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Persistors
{
    [ServiceLocate(typeof(IProjectPersistor))]
    public class ProjectPersistor : IProjectPersistor
    {
        private readonly DomainDbContext _dbContext;

        public ProjectPersistor(DomainDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid?> CreateProject(Project project)
        {
            var newProjectEntity = NewProjectDatabaseEntity(project);
            _dbContext.T_PROJECTs.Add(newProjectEntity);

            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

            return newProjectEntity.ID;
        }

        public async Task<Guid?> CreateRequirement(Requirement requirement, Guid projectId)
        {
            var newRequirementEntity = NewRequirementDatabaseEntity(requirement, projectId);
            _dbContext.T_REQUIREMENTs.Add(newRequirementEntity);

            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

            return newRequirementEntity.ID;
        }

        public async Task<Guid?> UpdateRequirement(Requirement requirement)
        {
            var rowrequirement = await _dbContext.T_REQUIREMENTs.FindAsync(requirement.ID).ConfigureAwait(false);
            DomainEntityNotFoundException.ThrowIfNull(requirement.ID, rowrequirement);

            MapToRequirementDatabaseEntity(rowrequirement, requirement);

            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

            return requirement.ID;
        }

        private T_PROJECT NewProjectDatabaseEntity(Project project)
        {
            var newProject = new T_PROJECT
            {
                ID = project.ID,
                NAME = project.Name,
                DESCRIPTION = project.Description,
                CREATED_UTC = project.CreatedOnUtc
            };

            return newProject;
        }

        private void MapToRequirementDatabaseEntity(T_REQUIREMENT rowrequirement, Requirement requirement)
        {
            rowrequirement.DESCRIPTION = requirement.Description;
            rowrequirement.CREATE_UTC = requirement.CreatedOnUtc;
        }

        private T_REQUIREMENT NewRequirementDatabaseEntity(Requirement requirement, Guid projectId)
        {
            var newRequirement = new T_REQUIREMENT
            {
                ID = requirement.ID,
                DESCRIPTION = requirement.Description,
                CREATE_UTC = requirement.CreatedOnUtc,
                PROJECT_ID = projectId
            };

            return newRequirement;
        }
    }
}