using Activator.DomainDrivenDesigner.Domain.Entities;

namespace Activator.DomainDrivenDesigner.Server.Models;

public record UpsertBusinessModelModel(string? id, string name, string rawDescription, List<BusinessModelProperty> properties);