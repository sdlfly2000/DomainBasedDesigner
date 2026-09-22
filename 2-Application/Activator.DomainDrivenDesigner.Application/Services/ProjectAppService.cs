using Activator.DomainDrivenDesigner.Application.AppRequests;
using Activator.DomainDrivenDesigner.Application.AppResponses;
using Activator.DomainDrivenDesigner.Domain.Project;
using Activator.DomainDrivenDesigner.Domain.Project.Entities;
using Common.Core.AOP.LogTrace;
using Common.Core.DependencyInjection;

namespace Activator.DomainDrivenDesigner.Application.Services;

[ServiceLocate(typeof(ProjectAppService))]
public class ProjectAppService
{
    private readonly IProjectRepository _projectRepository;
    private readonly IProjectPersistor _projectPersistor;
    private readonly IServiceProvider _serviceProvider;

    public ProjectAppService(IProjectRepository projectRepository, IProjectPersistor projectPersistor, IServiceProvider serviceProvider)
    {
        _projectRepository = projectRepository;
        _projectPersistor = projectPersistor;
        _serviceProvider = serviceProvider;
    }

    [LogTrace(typeof(CreateProjectAppResponse))]
    public async Task<CreateProjectAppResponse> Create(CreateProjectAppRequest request)
    {
        var newProject = Project.Create(request.ProjectName, request.ProjectDescription);

        await _projectPersistor.CreateProject(newProject).ConfigureAwait(false);

        return new CreateProjectAppResponse(request.Id, true, null);
    }

    [LogTrace(typeof(RetrieveFullProjectAppResponse))]
    public async Task<RetrieveFullProjectAppResponse> RetrieveFullProjects(RetrieveFullProjectAppRequest request)
    {
        var projects = await _projectRepository.RetrieveFullProjects().ConfigureAwait(false);

        return new RetrieveFullProjectAppResponse(request.Id, projects, true, null);
    }
}