using Activator.DomainDrivenDesigner.Support.Core.Marks;

namespace Activator.DomainDrivenDesigner.Domain.Project.Entities;

public class Project(Guid ID, string ProjectName) : EntityBase(ID)
{
    public List<Requirement> Requirements { get; set; } = [];

    public string Name { get; } = ProjectName;

    public string? Description { get; set; }

    public string? BaseDirectory { get; set; }

    public static Project Create(string ProjectName, string ProjectDescription, string? BaseDirectory = null)
    {
        return new Project(Guid.NewGuid(), ProjectName)
        {
            Description = ProjectDescription,
            BaseDirectory = BaseDirectory,
            CreatedOnUtc = DateTime.UtcNow,
        };
    }
}
