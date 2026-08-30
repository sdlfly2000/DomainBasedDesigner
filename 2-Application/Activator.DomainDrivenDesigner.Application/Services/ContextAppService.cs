using Activator.DomainDrivenDesigner.Application.AppRequests;
using Activator.DomainDrivenDesigner.Application.AppResponses;
using Activator.DomainDrivenDesigner.Domain.Repositories;
using Common.Core.AOP.LogTrace;
using Common.Core.DependencyInjection;

namespace Activator.DomainDrivenDesigner.Application.Services;

[ServiceLocate(typeof(ContextAppService))]
public class ContextAppService
{
    private readonly IDDDRepository _repository;
    private readonly IServiceProvider _serviceProvider;

    public ContextAppService(IDDDRepository repository, IServiceProvider serviceProvider)
    {
        _repository = repository;
        _serviceProvider = serviceProvider;
    }

    [LogTrace(returnType: typeof(RetrieveContextAppResponse))]
    public async Task<RetrieveContextAppResponse> RetrieveContexts(RetrieveContextAppRequest request)
    {
        var contexts = await _repository.RetrieveContexts().ConfigureAwait(false);

        return new RetrieveContextAppResponse(request.Id, contexts, true, null);
    }

    [LogTrace(returnType: typeof(CreateContextAppResponse))]
    public async Task<CreateContextAppResponse> CreateContext(CreateContextAppRequest request)
    {
        var contextId = await _repository.CreateContext(request.Name).ConfigureAwait(false);

        return new CreateContextAppResponse(request.Id, contextId, true, null);
    }
}
