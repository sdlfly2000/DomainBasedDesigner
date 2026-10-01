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
            var rows = await _dbContext.T_PROJECTs.Include(p => p.T_REQUIREMENTs).ToListAsync().ConfigureAwait(false);
            return rows.Select(MapToProjectDomainModel).ToList();
        }

        public async Task<Project> RetrieveProjectById(Guid projectId)
        {
            var row = await _dbContext.T_PROJECTs
                .Include(p => p.T_REQUIREMENTs)
                .FirstOrDefaultAsync(p => p.ID == projectId).ConfigureAwait(false);

            DomainEntityNotFoundException.ThrowIfNull(projectId, row);

            return MapToProjectDomainModel(row);
        }

        public async Task<List<Requirement>> RetrieveRequirementByProjectId(Guid projectId)
        {
            var rows = await _dbContext.T_PROJECTs
                .Include(p => p.T_REQUIREMENTs)
                .FirstOrDefaultAsync(p => p.ID == projectId).ConfigureAwait(false);

            DomainEntityNotFoundException.ThrowIfNull(projectId, rows);

            return rows.T_REQUIREMENTs.Select(MapToRequirementDomainModel).ToList();
        }

        public async Task<Requirement> RetrieveRequirementById(Guid requirementId)
        {
            var row = await _dbContext.T_REQUIREMENTs
                .FirstOrDefaultAsync(r => r.ID == requirementId).ConfigureAwait(false);

            DomainEntityNotFoundException.ThrowIfNull(requirementId, row);

            return MapToRequirementDomainModel(row);
        }

        private Project MapToProjectDomainModel(T_PROJECT rowProject)
        {
            var project = new Project(
                ID: rowProject.ID,
                ProjectName: rowProject.NAME
            )
            {
                Description = rowProject.DESCRIPTION,
                BaseDirectory = rowProject.BASE_DIRECTORY,
                CreatedOnUtc = rowProject.CREATED_UTC,
                Requirements = rowProject.T_REQUIREMENTs.Select(MapToRequirementDomainModel).ToList()
            };

            return project;
        }

        private Requirement MapToRequirementDomainModel(T_REQUIREMENT rowRequirment)
        {
            var requirement = new Requirement(
                ID: rowRequirment.ID
            )
            {
                Description = rowRequirment.DESCRIPTION,
                CreatedOnUtc = rowRequirment.CREATE_UTC
            };

            return requirement;
        }
    }
}