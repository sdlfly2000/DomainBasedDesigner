using Activator.DomainDrivenDesigner.Application.AppRequests;
using Activator.DomainDrivenDesigner.Application.AppResponses;
using Activator.DomainDrivenDesigner.Domain.Repositories;
using Common.Core.AOP.LogTrace;
using Common.Core.DependencyInjection;

namespace Activator.DomainDrivenDesigner.Application.Services;

[ServiceLocate(default)]
public class RequirementAppService
{
    private readonly IDDDRepository _repository;

    public RequirementAppService(IDDDRepository repository)
    {
        _repository = repository;
    }

    [LogTrace(returnType: typeof(RetrieveRequirementByProjectAppResponse))]
    public async Task<RetrieveRequirementByProjectAppResponse> RetrieveFullRequirements(RetrieveRequirementByProjectAppRequest request)
    {
        var requirements = await _repository.RetrieveRequirementByProjectId(request.ProjectId).ConfigureAwait(false);

        return requirements != null
            ? new RetrieveRequirementByProjectAppResponse(request.RequestId, requirements, true, null)
            : new RetrieveRequirementByProjectAppResponse(request.RequestId, null, false, "Failed to retrieve requirements");
    }
}
