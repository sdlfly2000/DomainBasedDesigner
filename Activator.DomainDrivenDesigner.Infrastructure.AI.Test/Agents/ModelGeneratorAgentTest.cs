using Activator.DomainDrivenDesigner.Infrastructure.AI.Agents;
using Activator.DomainDrivenDesigner.Infrastructure.AI.Client;
using FluentAssertions;
using Microsoft.Extensions.Options;

namespace Activator.DomainDrivenDesigner.Infrastructure.AI.Test.Agents;

public class ModelGeneratorAgentTest
{
    private ModelGeneratorAgent _modelGeneratorAgent;

    [SetUp]
    public void Setup()
    {
        var aiOptions = Options.Create(new AIOptions
        {
            Endpoint = "http://homeserver4:11434"
        });

        var aIAgentClientFactory = new AIAgentClientFactory(aiOptions);

        _modelGeneratorAgent = new ModelGeneratorAgent(aIAgentClientFactory, "ornith:9b");
    }

    [Test]
    public async Task Convert_ShouldReturnConvertedClasses_WhenValidMermaidDiagramIsProvided()
    {
        // Arrange
        var mermaidDiagram = @"
            ClassDiagram
                class User {
                    +Id: Guid
                    +Name: String
                    +Email: String
                }

                class Product {
                    +Id: Guid
                    +Name: String 
                    +Price: Decimal
                }
            ";

        // Action
        var result = await _modelGeneratorAgent.Create(mermaidDiagram, CancellationToken.None).ConfigureAwait(false);

        // Assert
        result.Should().NotBeNull();
    }
}
