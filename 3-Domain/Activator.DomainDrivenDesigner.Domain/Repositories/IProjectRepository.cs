using Activator.DomainDrivenDesigner.Domain.Entities;

namespace Activator.DomainDrivenDesigner.Domain.Repositories;

public interface IProjectRepository
{
    Task<Guid?> CreateProject(Project project);

    Task<List<Project>> RetrieveFullProjects();
}
