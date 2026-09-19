using Activator.DomainDrivenDesigner.Domain.Entities;
using Activator.DomainDrivenDesigner.Support.Core.Marks;

namespace Activator.DomainDrivenDesigner.Domain.Project.Entities;

public class Requirement(Guid ID) : EntityBase(ID)
{
    public string? Description { get; set; }

    public List<BusinessModel> BusinessModels { get; set; } = [];

    public List<BusinessAction> BusinessActions { get; set; } = [];
}
