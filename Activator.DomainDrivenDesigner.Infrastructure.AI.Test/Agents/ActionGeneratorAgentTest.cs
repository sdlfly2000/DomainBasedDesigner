using Activator.DomainDrivenDesigner.Infrastructure.AI.Agents;
using Activator.DomainDrivenDesigner.Infrastructure.AI.Client;
using FluentAssertions;
using Microsoft.Extensions.Options;

namespace Activator.DomainDrivenDesigner.Infrastructure.AI.Test.Agents;

public class ActionGeneratorAgentTest
{
    private ActionGeneratorAgent _actionGeneratorAgent;

    [SetUp]
    public void Setup()
    {
        var aiOptions = Options.Create(new AIOptions
        {
            Endpoint = "http://homeserver4:11434"
        });

        var aIAgentClientFactory = new AIAgentClientFactory(aiOptions);

        //_actionGeneratorAgent = new ActionGeneratorAgent(aIAgentClientFactory, "ornith:9b");
        _actionGeneratorAgent = new ActionGeneratorAgent(aIAgentClientFactory);
    }

    [Test]
    public async Task Convert_ShouldReturnConvertedClasses_WhenValidInstructionIsProvided()
    {
        // Arrange
        var instruction =
            """
            Generate complete C# code for file **Domain/User/UserService.cs**

            Rules:
            1. Class: **UserService**
            2. Inject **IUserRepository** through constructor
            3. Public async method signature: Task<User> GetUser(Guid id)
            4. Method logic: 
            ```mermaid
                flowchart TB
                    subgraph main [Application.Service.GetUser]
                        direction TB
                        start(("Start -> [UserId: Guid]")) -->
                        FindUser["Find User By UserId -> [IUserRepository.GetUserById(id)]"] -->
                        check1{Found?} --"no""-->
                        userNotFound["Throw Not found exception""]      
                        check1 --""yes""--> 
                        return[return User] -->
                        terminal(End)
                    end
            ```
            Reference interface:
            public interface IUserRepository
            {
                Task<User> GetUserById(Guid id);
            }

            Only output full source code of UserService.cs, no other text. Use async/await correctly.
            """;

        // Action
        var result = await _actionGeneratorAgent.Create(instruction, CancellationToken.None).ConfigureAwait(false);

        // Assert
        result.Should().NotBeNull();
        Console.WriteLine(string.Concat("File: ", result.Result.file_path));
        Console.WriteLine(string.Concat("Content: ", Environment.NewLine, result.Result.content));
    }
}
