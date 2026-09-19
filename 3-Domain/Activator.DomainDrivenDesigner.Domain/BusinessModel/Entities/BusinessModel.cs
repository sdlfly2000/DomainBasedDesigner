using Activator.DomainDrivenDesigner.Support.Core.Marks;

namespace Activator.DomainDrivenDesigner.Domain.BusinessModel.Entities;

public class BusinessModel(Guid ID) : EntityBase(ID)
{
    public string? Name { get; set; }

    public string? ContentMermaid { get; set; }

    public Guid? ContextId { get; set; }    
}
