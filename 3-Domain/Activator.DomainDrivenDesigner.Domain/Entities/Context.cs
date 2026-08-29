using Activator.DomainDrivenDesigner.Support.Core.Marks;

namespace Activator.DomainDrivenDesigner.Domain.Entities;

public class Context(Guid ID) : EntityBase(ID)
{
    public string? Name { get; set; }
}
