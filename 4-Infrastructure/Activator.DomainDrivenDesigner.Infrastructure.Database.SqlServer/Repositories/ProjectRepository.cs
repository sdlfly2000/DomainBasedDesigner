using Activator.DomainDrivenDesigner.Domain.Project;
using Activator.DomainDrivenDesigner.Domain.Project.Entities;
using Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Context;
using Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Entities;
using Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Exceptions;
using Common.Core.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            var rows = await _dbContext.T_REQUIREMENTs
                .Where(r => r.PROJECT_ID == projectId)
                .ToListAsync().ConfigureAwait(false);

            return rows.Select(MapToRequirementDomainModel).ToList();
        }

        private Project MapToProjectDomainModel(T_PROJECT rowProject)
        {
            var project = new Project(rowProject.ID, rowProject.NAME)
            {
                Description = rowProject.DESCRIPTION,
                CreatedOnUtc = rowProject.CREATED_UTC
            };

            foreach (var requirementRow in rowProject.T_REQUIREMENTs)
            {
                project.Requirements.Add(MapToRequirementDomainModel(requirementRow));
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

            // Do NOT load nested T_BUSINESS_ACTION nor T_BUSINESS_MODEL in T_REQUIREMENT

            return requirement;
        }
    }
}