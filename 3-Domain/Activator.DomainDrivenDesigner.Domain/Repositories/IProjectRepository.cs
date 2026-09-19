namespace Activator.DomainDrivenDesigner.Domain.Repositories;

public interface IProjectRepository
{
    Task<Guid?> CreateProject(Project.Entities.Project project);

    Task<List<Project.Entities.Project>> RetrieveFullProjects();
}
