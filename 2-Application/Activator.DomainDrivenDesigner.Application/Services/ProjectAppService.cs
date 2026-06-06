using Activator.DomainDrivenDesigner.Application.AppRequests;
using Activator.DomainDrivenDesigner.Application.AppResponses;
using Activator.DomainDrivenDesigner.Domain.Entities;
using Activator.DomainDrivenDesigner.Domain.Repositories;
using Common.Core.AOP.LogTrace;
using Common.Core.DependencyInjection;

namespace Activator.DomainDrivenDesigner.Application.Services;

[ServiceLocate(default)]
public class ProjectAppService
{
    private readonly IDDDRepository _repository;

    public ProjectAppService(IDDDRepository repository)
    {
        _repository = repository;
    }

    [LogTrace(returnType: typeof(CreateProjectAppResponse))]
    public async Task<CreateProjectAppResponse> Create(CreateProjectAppRequest request)
    {
        var project = Project.Create(request.ProjectName, request.ProjectDescription);

        var projectId = await _repository.CreateProject(project).ConfigureAwait(false);

        return projectId != null
            ? new CreateProjectAppResponse(request.Id, true, null)
            : new CreateProjectAppResponse(request.Id, false, "Failed to create project");
    }

    [LogTrace(returnType: typeof(RetrieveFullProjectAppResponse))]
    public async Task<RetrieveFullProjectAppResponse> RetrieveFullProjects(RetrieveFullProjectAppRequest request)
    {
        var projects = await _repository.RetrieveFullProjects().ConfigureAwait(false);

        return projects != null
            ? new RetrieveFullProjectAppResponse(request.Id, projects, true, null)
            : new RetrieveFullProjectAppResponse(request.Id, null, false, "Failed to retrieve projects");
    }

    [LogTrace(returnType: typeof(RetrieveBusinessModelsAppResponse))]
    public async Task<RetrieveBusinessModelsAppResponse> RetrieveProjectBusinessModels(RetrieveBusinessModelsAppRequest request)
    {
        var businessModels = await _repository.RetrieveBusinessModelsByProjectId(request.ProjectId).ConfigureAwait(false);

        return businessModels != null && businessModels.Count > 0
            ? new RetrieveBusinessModelsAppResponse(request.Id, businessModels, true, null)
            : new RetrieveBusinessModelsAppResponse(request.Id, businessModels, false, "No business models found for the specified project");
    }
}
