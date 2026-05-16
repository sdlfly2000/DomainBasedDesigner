using Activator.DomainDrivenDesigner.Domain.Entities;
using Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Context;
using Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Repositories;
using Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Exceptions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Activator.DomainDrivenDesigner.Application.Tests.Infrastructure;

public class DDDRepositoryTest
{
    private DomainDbContext CreateSqlServerContext()
    {
        var connectionString = "Server=homeserver2;Database=DomainDrivenDesigner;User Id=sdlfly2000;Password=sdl@1215;TrustServerCertificate=True;";

        var options = new DbContextOptionsBuilder<DomainDbContext>()
            .UseSqlServer(connectionString)
            .Options;

        return new DomainDbContext(options);
    }

    [Test]
    public async Task CreateRequirement_WithValidProjectAndRequirement_ShouldCreateRequirementAndLinkToProject()
    {
        // Arrange
        var context = CreateSqlServerContext();
        var repository = new DDDRepository(context);

        var projectId = Guid.NewGuid();
        var project = new Project(projectId, "Test Project");
        project.CreatedOnUTC = DateTime.UtcNow;

        await repository.CreateProject(project);

        var requirementId = Guid.NewGuid();
        var requirement = new Requirement(requirementId)
        {
            Description = "Test Requirement",
            CreatedOnUTC = DateTime.UtcNow
        };

        // Act
        var result = await repository.CreateRequirement(requirement, projectId);

        // Assert
        result.Should().Be(requirementId);

        // Verify requirement is stored
        var storedRequirement = await context.T_REQUIREMENTs
            .FirstOrDefaultAsync(r => r.ID == requirementId);

        storedRequirement.Should().NotBeNull();
        storedRequirement!.DESCRIPTION.Should().Be("Test Requirement");
        storedRequirement!.PROJECT_ID.Should().Be(projectId);

        // Verify requirement is linked to project
        var storedProject = await context.T_PROJECTs
            .Include(p => p.T_REQUIREMENTs)
            .FirstOrDefaultAsync(p => p.ID == projectId);

        storedProject.Should().NotBeNull();
        storedProject!.T_REQUIREMENTs.Should().Contain(r => r.ID == requirementId);
    }

    [Test]
    public void CreateRequirement_WithNonExistentProject_ShouldThrowDomainEntityNotFoundException()
    {
        // Arrange
        var context = CreateSqlServerContext();
        var repository = new DDDRepository(context);

        var nonExistentProjectId = Guid.NewGuid();
        var requirement = new Requirement(Guid.NewGuid())
        {
            Description = "Test Requirement",
            CreatedOnUTC = DateTime.UtcNow
        };

        // Act & Assert
        Assert.ThrowsAsync<DomainEntityNotFoundException>(
            () => repository.CreateRequirement(requirement, nonExistentProjectId)
        );
    }
}

