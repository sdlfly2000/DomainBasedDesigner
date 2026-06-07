using Activator.DomainDrivenDesigner.Application.AppRequests;
using Activator.DomainDrivenDesigner.Application.Services;
using Activator.DomainDrivenDesigner.Domain.Entities;
using Activator.DomainDrivenDesigner.Domain.Repositories;
using FakeItEasy;
using FluentAssertions;

namespace Activator.DomainDrivenDesigner.Application.Tests.Application;

public class ProjectAppServiceTests
{
    private IDDDRepository _repository = null!;
    private ProjectAppService _service = null!;

    [SetUp]
    public void Setup()
    {
        _repository = A.Fake<IDDDRepository>();
        _service = new ProjectAppService(_repository);
    }

    [Test]
    public async Task Create_ShouldReturnSuccess_WhenProjectIsCreated()
    {
        // Arrange
        var requestId = Guid.NewGuid();
        var projectName = "Test Project";
        var projectDescription = "Test Description";
        var request = new CreateProjectAppRequest(requestId, projectName, projectDescription);
        var projectId = Guid.NewGuid();

        A.CallTo(() => _repository.CreateProject(A<Project>.Ignored)).Returns(projectId);

        // Act
        var response = await _service.Create(request);

        // Assert
        response.RequestId.Should().Be(requestId);
        response.Success.Should().BeTrue();
        response.ErrorMessage.Should().BeNull();
        A.CallTo(() => _repository.CreateProject(A<Project>.That.Matches(p => p.Name == projectName))).MustHaveHappenedOnceExactly();
    }

    [Test]
    public async Task Create_ShouldReturnFailure_WhenProjectCreationFails()
    {
        // Arrange
        var requestId = Guid.NewGuid();
        var request = new CreateProjectAppRequest(requestId, "Test Project", "Test Description");

        A.CallTo(() => _repository.CreateProject(A<Project>.Ignored)).Returns((Guid?)null);

        // Act
        var response = await _service.Create(request);

        // Assert
        response.Success.Should().BeFalse();
        response.ErrorMessage.Should().Be("Failed to create project");
    }

    [Test]
    public async Task RetrieveFullProjects_ShouldReturnProjects_WhenProjectsExist()
    {
        // Arrange
        var requestId = Guid.NewGuid();
        var request = new RetrieveFullProjectAppRequest(requestId);
        var projects = new List<Project> { Project.Create("Project 1", "Description 1"), Project.Create("Project 2", "Description 2") };

        A.CallTo(() => _repository.RetrieveFullProjects()).Returns(projects);

        // Act
        var response = await _service.RetrieveFullProjects(request);

        // Assert
        response.RequestId.Should().Be(requestId);
        response.Success.Should().BeTrue();
        response.Projects.Should().HaveCount(2);
        response.Projects.Should().BeEquivalentTo(projects);
        response.ErrorMessage.Should().BeNull();
    }

    [Test]
    public async Task RetrieveFullProjects_ShouldReturnFailure_WhenRepositoryReturnsNull()
    {
        // Arrange
        var requestId = Guid.NewGuid();
        var request = new RetrieveFullProjectAppRequest(requestId);

        A.CallTo(() => _repository.RetrieveFullProjects()).Returns((List<Project>)null!);

        // Act
        var response = await _service.RetrieveFullProjects(request);

        // Assert
        response.Success.Should().BeFalse();
        response.Projects.Should().BeNull();
        response.ErrorMessage.Should().Be("Failed to retrieve projects");
    }
}
