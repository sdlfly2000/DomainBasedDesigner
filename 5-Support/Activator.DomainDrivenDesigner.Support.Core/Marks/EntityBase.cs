namespace Activator.DomainDrivenDesigner.Support.Core.Marks;

public abstract class EntityBase
{
    protected EntityBase(Guid ID) => this.ID = ID;

    public Guid ID { get; set; }

    public DateTime CreatedOnUtc { get; set; }
}
