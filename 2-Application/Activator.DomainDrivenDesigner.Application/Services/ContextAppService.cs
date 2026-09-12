using Activator.DomainDrivenDesigner.Application.AppRequests;
using Activator.DomainDrivenDesigner.Application.AppResponses;
using Activator.DomainDrivenDesigner.Domain.Repositories;
using Common.Core.AOP.LogTrace;
using Common.Core.DependencyInjection;

namespace Activator.DomainDrivenDesigner.Application.Services;

[ServiceLocate(typeof(ContextAppService))]
public class ContextAppService
{
    private readonly IContextRepository _contextRepository;
    private readonly IServiceProvider _serviceProvider;

    public ContextAppService(
        IContextRepository contextRepository,
        IServiceProvider serviceProvider)
    {
        _contextRepository = contextRepository;
        _serviceProvider = serviceProvider;
    }

    [LogTrace(typeof(RetrieveContextAppResponse))]
    public async Task<RetrieveContextAppResponse> RetrieveContexts(RetrieveContextAppRequest request)
    {
        var contexts = await _contextRepository.RetrieveContexts(request.ProjectId).ConfigureAwait(false);
        return new RetrieveContextAppResponse(Guid.Empty, contexts, true, null);
    }

    [LogTrace(typeof(CreateContextAppResponse))]
    public async Task<CreateContextAppResponse> CreateContext(CreateContextAppRequest request)
    {
        var context = await _contextRepository.CreateContext(request.Name, request.ProjectId).ConfigureAwait(false);
        return new CreateContextAppResponse(Guid.Empty, context, true, null);
    }
}