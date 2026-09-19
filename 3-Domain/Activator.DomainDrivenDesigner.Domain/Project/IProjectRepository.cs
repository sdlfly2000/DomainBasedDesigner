namespace Activator.DomainDrivenDesigner.Domain.Project;

public interface IProjectRepository
{
    Task<Guid?> CreateProject(Entities.Project project);

    Task<List<Entities.Project>> RetrieveFullProjects();
}
