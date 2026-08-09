using Activator.DomainDrivenDesigner.Support.Core.Marks;

namespace Activator.DomainDrivenDesigner.Domain.Entities;

public class BusinessModel(Guid ID) : EntityBase(ID)
{
    public string? Name { get; set; }

    public string? RawDescription { get; set; }

    public string? Context { get; set; }

    public Guid? ContextId { get; set; }
    
    public List<BusinessModelProperty> Properties { get; set; } = [];
}
