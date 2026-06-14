using Activator.DomainDrivenDesigner.Application.AppRequests;
using Activator.DomainDrivenDesigner.Application.AppResponses;
using Activator.DomainDrivenDesigner.Domain.Entities;
using Activator.DomainDrivenDesigner.Domain.Repositories;
using Activator.DomainDrivenDesigner.Infrastructure.AI.Agents;
using Common.Core.AOP.LogTrace;
using Common.Core.DependencyInjection;
using System.Text.Json;

namespace Activator.DomainDrivenDesigner.Application.Services;

[ServiceLocate(default)]
public class RequirementAppService(
    IDDDRepository repository,
    SemanticAnalysisAgent semanticAnalysisAgent,
    MermaidConverterAgent mermaidConverterAgent,
    IServiceProvider serviceProvider)
{
    private readonly IDDDRepository _repository = repository;
    private readonly SemanticAnalysisAgent _semanticAnalysisAgent = semanticAnalysisAgent;
    private readonly MermaidConverterAgent _mermaidConverterAgent = mermaidConverterAgent;
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    [LogTrace(returnType: typeof(RetrieveRequirementByProjectAppResponse))]
    public async Task<RetrieveRequirementByProjectAppResponse> RetrieveFullRequirements(RetrieveRequirementByProjectAppRequest request)
    {
        var requirements = await _repository.RetrieveRequirementByProjectId(request.ProjectId).ConfigureAwait(false);

        return requirements != null
            ? new RetrieveRequirementByProjectAppResponse(request.RequestId, requirements, true, null)
            : new RetrieveRequirementByProjectAppResponse(request.RequestId, null, false, "Failed to retrieve requirements");
    }


    [LogTrace(returnType: typeof(AnalyzeRequirementsResponse))]
    public async Task<AnalyzeRequirementsResponse> AnalyzeRequirement(AnalyzeRequirementsRequest request)
    {
        var semanticAnalysisResponse = await _semanticAnalysisAgent.Analyze(request.RequirementDescription).ConfigureAwait(false);
        
        var models = semanticAnalysisResponse.Result.nouns.Select(n => new BusinessModel(Guid.NewGuid()) { Name = n }).ToArray();
        var actions = semanticAnalysisResponse.Result.verbs.Select(v => new BusinessAction(Guid.NewGuid()) { Name = v }).ToArray();

        foreach (var relationship in semanticAnalysisResponse.Result.relationships)
        {
            if (semanticAnalysisResponse.Result.nouns.Contains(relationship.noun))
            {
                relationship.modifiers.ToList().ForEach(modifier =>
                {
                    var model = models.SingleOrDefault(m => m.Name == relationship.noun);
                    model?.Properties.Add(new BusinessModelProperty(Guid.NewGuid()) { Name = modifier });
                });
            }
        }

        var serializedModel = JsonSerializer.Serialize(models);
        var mermaidResponse = await _mermaidConverterAgent.Convert(serializedModel).ConfigureAwait(false);

        return semanticAnalysisResponse != null
            ? new AnalyzeRequirementsResponse(
                request.RequestId,
                models,
                actions,
                mermaidResponse.Text,
                true, 
                null)
            : new AnalyzeRequirementsResponse(request.RequestId, null, null, "", false, "Failed to analyze requirement");
    }
}
