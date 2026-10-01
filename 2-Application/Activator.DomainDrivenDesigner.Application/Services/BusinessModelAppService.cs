using Activator.DomainDrivenDesigner.Application.AppRequests;
using Activator.DomainDrivenDesigner.Application.AppResponses;
using Activator.DomainDrivenDesigner.Domain.BusinessModel;
using Activator.DomainDrivenDesigner.Domain.BusinessModel.Entities;
using Common.Core.AOP.LogTrace;
using Common.Core.DependencyInjection;

namespace Activator.DomainDrivenDesigner.Application.Services;

[ServiceLocate(typeof(BusinessModelAppService))]
public class BusinessModelAppService
{
    private readonly IBusinessModelRepository _businessModelRepository;
    private readonly IBusinessModelPersistor _businessModelPersistor;
    private readonly IServiceProvider _serviceProvider;

    public BusinessModelAppService(
        IBusinessModelRepository businessModelRepository,
        IBusinessModelPersistor businessModelPersistor,
        IServiceProvider serviceProvider)
    {
        _businessModelRepository = businessModelRepository;
        _businessModelPersistor = businessModelPersistor;
        _serviceProvider = serviceProvider;
    }

    [LogTrace(typeof(RetrieveBusinessModelByIdAppResponse))]
    public async Task<RetrieveBusinessModelByIdAppResponse> RetrieveBusinessModelById(RetrieveBusinessModelByIdAppRequest request)
    {
        var businessModel = await _businessModelRepository.LoadBusinessModelById(request.ModelId).ConfigureAwait(false);
        return new RetrieveBusinessModelByIdAppResponse(
            request.Id,
            businessModel,
            businessModel != null,
            businessModel == null ? "Business model not found" : null);
    }

    [LogTrace(typeof(UpsertBusinessModelsAppResponse))]
    public async Task<UpsertBusinessModelsAppResponse> UpsertProjectBusinessModels(UpsertBusinessModelsAppRequest request)
    {
        var newRetrieveBusinessModelByIdAppRequest = new RetrieveBusinessModelByIdAppRequest(request.Id, request.Model.ID);
        var businessModel = await RetrieveBusinessModelById(newRetrieveBusinessModelByIdAppRequest).ConfigureAwait(false);

        if (businessModel.Success && businessModel.BusinessModel != null)
        {
            await _businessModelPersistor.UpdateBusinessModel(businessModel.BusinessModel).ConfigureAwait(false);
        }
        else
        {
            await _businessModelPersistor.CreateBusinessModel(request.Model, request.RequirementId).ConfigureAwait(false);
        }

        return new UpsertBusinessModelsAppResponse(
            request.Id,
            true,
            null);
    }

    [LogTrace(typeof(RetrieveBusinessModelByNameAppResponse))]
    public async Task<RetrieveBusinessModelByNameAppResponse> RetrieveBusinessModelByName(RetrieveBusinessModelsByNameAppRequest request)
    {
        var businessModels = await _businessModelRepository.LoadBusinessModelsByRequirementId(request.RequirementId).ConfigureAwait(false);
        var filteredBusinessModel = businessModels.FirstOrDefault(b => b.Name == request.ModelName);

        return new RetrieveBusinessModelByNameAppResponse(
            request.Id,
            filteredBusinessModel,
            filteredBusinessModel != null,
            filteredBusinessModel == null ? "Business model not found" : null);
    }
}