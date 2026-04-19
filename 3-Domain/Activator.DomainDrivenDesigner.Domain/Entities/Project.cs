using Activator.DomainDrivenDesigner.Support.Core.Marks;

namespace Activator.DomainDrivenDesigner.Domain.Entities;

public class Project(Guid ID) : EntityBase(ID)
{
    public List<Requirement> Requirements { get; set; } = [];
}
