namespace Activator.DomainDrivenDesigner.Domain.Project;

public interface IProjectRepository
{
    Task<List<Entities.Project>> RetrieveFullProjects();

    Task<Entities.Project> RetrieveProjectById(Guid projectId);
}
