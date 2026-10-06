using Activator.DomainDrivenDesigner.Support.Core.Marks;

namespace Activator.DomainDrivenDesigner.Domain.Project.Entities;

public class Requirement(Guid ID) : EntityBase(ID)
{
    public List<Guid> BusinessActionIds { get; } = new List<Guid>();
    public List<Guid> BusinessModelIds { get; } = new List<Guid>();

    public string Description { get; set; }
}