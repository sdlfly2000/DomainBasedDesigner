using Activator.DomainDrivenDesigner.Domain.Enum;
using Activator.DomainDrivenDesigner.Support.Core.Marks;

namespace Activator.DomainDrivenDesigner.Domain.Entities;

public class BusinessModelProperty(Guid ID) : EntityBase(ID)
{
    public string? Name { get; set; }
    
    public ModelPropertyType? Type { get; set; }
}
