using Activator.DomainDrivenDesigner.Application.AppRequests;
using Activator.DomainDrivenDesigner.Application.AppResponses;
using Activator.DomainDrivenDesigner.Domain.Entities;
using Activator.DomainDrivenDesigner.Domain.Repositories;

namespace Activator.DomainDrivenDesigner.Application.Services;

public class ProjectAppService
{
    private readonly IDDDRepository _repository;

    public ProjectAppService(IDDDRepository repository)
    {
        _repository = repository;
    }

    public async Task<CreateProjectAppResponse> Create(CreateProjectAppRequest request)
    {
        var project = Project.Create(request.ProjectName);

        var projectId = await _repository.CreateProject(project).ConfigureAwait(false);

        return projectId != null
            ? new CreateProjectAppResponse(request.Id, true, null)
            : new CreateProjectAppResponse(request.Id, false, "Failed to create project");
    }
}
