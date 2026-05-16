using Activator.DomainDrivenDesigner.Application.AppRequests;
using Activator.DomainDrivenDesigner.Application.Services;
using Activator.DomainDrivenDesigner.Domain.Entities;
using Activator.DomainDrivenDesigner.Domain.Repositories;
using FluentAssertions;
using NSubstitute;

namespace Activator.DomainDrivenDesigner.Application.Tests;

public class ProjectAppServiceTests
{
    private readonly IDDDRepository _repository;
    private readonly ProjectAppService _service;

    public ProjectAppServiceTests()
    {
        _repository = Substitute.For<IDDDRepository>();
        _service = new ProjectAppService(_repository);
    }

    [Fact]
    public async Task Create_ShouldReturnSuccess_WhenProjectIsCreated()
    {
        // Arrange
        var requestId = Guid.NewGuid();
        var projectName = "Test Project";
        var request = new CreateProjectAppRequest(requestId, projectName);
        var projectId = Guid.NewGuid();

        _repository.CreateProject(Arg.Any<Project>()).Returns(projectId);

        // Act
        var response = await _service.Create(request);

        // Assert
        response.RequestId.Should().Be(requestId);
        response.Success.Should().BeTrue();
        response.ErrorMessage.Should().BeNull();
        await _repository.Received(1).CreateProject(Arg.Is<Project>(p => p.Name == projectName));
    }

    [Fact]
    public async Task Create_ShouldReturnFailure_WhenProjectCreationFails()
    {
        // Arrange
        var requestId = Guid.NewGuid();
        var request = new CreateProjectAppRequest(requestId, "Test Project");

        _repository.CreateProject(Arg.Any<Project>()).Returns((Guid?)null);

        // Act
        var response = await _service.Create(request);

        // Assert
        response.Success.Should().BeFalse();
        response.ErrorMessage.Should().Be("Failed to create project");
    }

    [Fact]
    public async Task RetrieveFullProjects_ShouldReturnProjects_WhenProjectsExist()
    {
        // Arrange
        var requestId = Guid.NewGuid();
        var request = new RetrieveFullProjectAppRequest(requestId);
        var projects = new List<Project> { Project.Create("Project 1"), Project.Create("Project 2") };

        _repository.RetrieveFullProjects().Returns(projects);

        // Act
        var response = await _service.RetrieveFullProjects(request);

        // Assert
        response.RequestId.Should().Be(requestId);
        response.Success.Should().BeTrue();
        response.Projects.Should().HaveCount(2);
        response.Projects.Should().BeEquivalentTo(projects);
        response.ErrorMessage.Should().BeNull();
    }

    [Fact]
    public async Task RetrieveFullProjects_ShouldReturnFailure_WhenRepositoryReturnsNull()
    {
        // Arrange
        var requestId = Guid.NewGuid();
        var request = new RetrieveFullProjectAppRequest(requestId);

        _repository.RetrieveFullProjects().Returns((List<Project>)null!);

        // Act
        var response = await _service.RetrieveFullProjects(request);

        // Assert
        response.Success.Should().BeFalse();
        response.Projects.Should().BeNull();
        response.ErrorMessage.Should().Be("Failed to retrieve projects");
    }
}
