using Activator.DomainDrivenDesigner.Support.Core.Marks;

namespace Activator.DomainDrivenDesigner.Domain.Context.Entities;

public class Context(Guid ID) : EntityBase(ID)
{
    public string? Name { get; set; }
}
