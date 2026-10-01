using Activator.DomainDrivenDesigner.Application.AppRequests;
using Activator.DomainDrivenDesigner.Application.AppResponses;
using Activator.DomainDrivenDesigner.Domain.BusinessModel;
using Activator.DomainDrivenDesigner.Domain.BusinessModel.Entities;
using Activator.DomainDrivenDesigner.Domain.Project;
using Activator.DomainDrivenDesigner.Domain.Project.Entities;
using Activator.DomainDrivenDesigner.Infrastructure.AI.Agents;
using Common.Core.AOP.LogTrace;
using Common.Core.DependencyInjection;
using System.Text.Json;

namespace Activator.DomainDrivenDesigner.Application.Services;

[ServiceLocate(default)]
public class RequirementAppService(
    IProjectRepository projectRepository,
    IProjectPersistor projectPersistor,
    IBusinessModelRepository businessModelRepository,
    IBusinessModelPersistor businessModelPersistor,
    SemanticAnalysisAgent semanticAnalysisAgent,
    MermaidConverterAgent mermaidConverterAgent,
    IServiceProvider serviceProvider)
{
    private readonly IProjectRepository _projectRepository = projectRepository;
    private readonly IProjectPersistor _projectPersistor = projectPersistor;
    private readonly IBusinessModelRepository _businessModelRepository = businessModelRepository;
    private readonly IBusinessModelPersistor _businessModelPersistor = businessModelPersistor;
    private readonly SemanticAnalysisAgent _semanticAnalysisAgent = semanticAnalysisAgent;
    private readonly MermaidConverterAgent _mermaidConverterAgent = mermaidConverterAgent;
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    [LogTrace(returnType: typeof(RetrieveRequirementByProjectAppResponse))]
    public async Task<RetrieveRequirementByProjectAppResponse> RetrieveFullRequirements(RetrieveRequirementByProjectAppRequest request)
    {
        var requirements = await _projectRepository.RetrieveRequirementByProjectId(request.ProjectId).ConfigureAwait(false);

        return requirements != null
            ? new RetrieveRequirementByProjectAppResponse(request.RequestId, requirements, true, null)
            : new RetrieveRequirementByProjectAppResponse(request.RequestId, null, false, "Failed to retrieve requirements");
    }


    [LogTrace(returnType: typeof(AnalyzeRequirementsResponse))]
    public async Task<AnalyzeRequirementsResponse> AnalyzeRequirement(AnalyzeRequirementsRequest request)
    {
        var semanticAnalysisResponse = await _semanticAnalysisAgent.Analyze(request.RequirementDescription).ConfigureAwait(false);
        var businessModels = semanticAnalysisResponse.Result.nouns.Select(noun => new BusinessModel(Guid.NewGuid()) { Name = noun }).ToArray(); ;
        var serializedModel = JsonSerializer.Serialize(semanticAnalysisResponse.Result.relationships);
        var mermaidResponse = await _mermaidConverterAgent.Convert(serializedModel).ConfigureAwait(false);

        return semanticAnalysisResponse != null
            ? new AnalyzeRequirementsResponse(
                request.RequestId,
                businessModels,
                mermaidResponse.Text,
                true, 
                null)
            : new AnalyzeRequirementsResponse(request.RequestId, Array.Empty<BusinessModel>(), string.Empty, false, "Failed to analyze requirement");
    }

    [LogTrace(returnType: typeof(SaveRequirementResponse))]
    public async Task<SaveRequirementResponse> SaveRequirement(SaveRequirementRequest request)
    {
        var requirement = request.RequirementId != null
            ? await _projectRepository.RetrieveRequirementById(request.RequirementId.Value).ConfigureAwait(false)
            : new Requirement(Guid.NewGuid());

        requirement.Description = request.RequirementDescription;

        if(request.RequirementId != null)
        {
            _ = await _projectPersistor.UpdateRequirement(requirement).ConfigureAwait(false);
        }
        else
        {
            _ = await _projectPersistor.CreateRequirement(requirement, request.ProjectId).ConfigureAwait(false);
        }

        return new SaveRequirementResponse(request.Id, true, string.Empty);
    }

    [LogTrace(returnType: typeof(RetrieveRequirementResponse))]
    public async Task<RetrieveRequirementResponse> RetrieveRequirement(Guid requestId, Guid requirementId)
    {   
        var requirement = await _projectRepository.RetrieveRequirementById(requirementId).ConfigureAwait(false);

        return new RetrieveRequirementResponse(requestId, requirement, true, string.Empty);
    }
}
