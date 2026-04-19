using Activator.DomainDrivenDesigner.Support.Core.Marks;

namespace Activator.DomainDrivenDesigner.Domain.Entities;

public class BusinessAction(Guid ID) : EntityBase(ID)
{
    public string? Name { get; set; }

    public BusinessAction? ParentBusinessAction;

    public List<BusinessAction> ChildBusinessActions { get; set; } = [];
}
