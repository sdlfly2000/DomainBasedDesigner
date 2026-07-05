namespace Activator.DomainDrivenDesigner.Server.Models;

public record SaveRequirementRequestModel(string projectId, string requirementId, string description);