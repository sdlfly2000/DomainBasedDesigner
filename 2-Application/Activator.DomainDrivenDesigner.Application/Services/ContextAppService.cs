using Activator.DomainDrivenDesigner.Application.AppRequests;
using Activator.DomainDrivenDesigner.Application.AppResponses;
using Activator.DomainDrivenDesigner.Domain.Context;
using Activator.DomainDrivenDesigner.Domain.Context.Entities;
using Common.Core.AOP.LogTrace;
using Common.Core.DependencyInjection;

namespace Activator.DomainDrivenDesigner.Application.Services
{
    [ServiceLocate(typeof(ContextAppService))]
    public class ContextAppService
    {
        private readonly IContextRepository _contextRepository;
        private readonly IServiceProvider _serviceProvider;

        public ContextAppService(IContextRepository contextRepository, IServiceProvider serviceProvider)
        {
            _contextRepository = contextRepository;
            _serviceProvider = serviceProvider;
        }

        [LogTrace(typeof(RetrieveContextAppResponse))]
        public async Task<RetrieveContextAppResponse> RetrieveContexts(RetrieveContextAppRequest request)
        {
            var contexts = await _contextRepository.RetrieveContexts(request.ProjectId).ConfigureAwait(false);
            return new RetrieveContextAppResponse(request.Id, contexts, true, null);
        }

        [LogTrace(typeof(CreateContextAppResponse))]
        public async Task<CreateContextAppResponse> CreateContext(CreateContextAppRequest request)
        {
            var newContext = new Context(Guid.NewGuid());
            newContext.Name = request.Name;
            await _contextRepository.CreateContext(newContext.Name, request.ProjectId).ConfigureAwait(false);
            return new CreateContextAppResponse(request.Id, newContext.ID, true, null);
        }
    }
}