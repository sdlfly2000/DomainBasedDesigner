namespace Activator.DomainDrivenDesigner.Server.Models;

public record UpsertBusinessModelModel(string? id, string name, string contentMermaid, string contextId, string contextName, DateTime? createdOnUtc);