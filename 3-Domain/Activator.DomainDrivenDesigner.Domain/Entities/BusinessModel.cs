using Activator.DomainDrivenDesigner.Domain.Enum;
using Activator.DomainDrivenDesigner.Support.Core.Marks;

namespace Activator.DomainDrivenDesigner.Domain.Entities;

public class BusinessModel(Guid ID) : EntityBase(ID)
{
    public string? Name { get; set; }
    
    public List<ModelPropertyType> Type { get; set; } = [];
}
