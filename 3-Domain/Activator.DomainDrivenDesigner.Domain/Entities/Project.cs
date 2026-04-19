using Activator.DomainDrivenDesigner.Support.Core.Marks;

namespace Activator.DomainDrivenDesigner.Domain.Entities;

public class Project(Guid ID, string ProjectName) : EntityBase(ID)
{
    public List<Requirement> Requirements { get; set; } = [];

    public string Name { get; } = ProjectName;

    public static Project Create(string ProjectName)
    {
        return new Project(Guid.NewGuid(), ProjectName);
    }
}
