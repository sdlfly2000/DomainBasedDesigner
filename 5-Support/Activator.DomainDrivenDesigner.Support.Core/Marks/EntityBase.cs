namespace Activator.DomainDrivenDesigner.Support.Core.Marks;

public abstract class EntityBase
{
    protected EntityBase(Guid ID)
    {   
        Id = ID;
    }

    public Guid Id { get; set; }

    public DateTime CreatedOnUTC { get; set; }
}
