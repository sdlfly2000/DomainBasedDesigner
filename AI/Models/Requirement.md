## Generate complete C# code 
Target File: **3-Domain//Activator.DomainDrivenDesigner.Domain//Project//Entities//Requirement.cs**

## Format:
- **Formatting Style:** Strictly use Allman style (opening braces `{` must always be placed on a new line for classes, methods, and control blocks).

- Write a C# **Requirement** class

    ```csharp
    namespace Activator.DomainDrivenDesigner.Domain.Project.Entities;

    public class Requirement(Guid ID) : EntityBase(ID)
    {
        // Your Full Code Implementation (Allman Style including Using statements)
    }
    ```

## Rules:
1. Class: **Requirement**, Inherits **EntityBase**, Use Primary Constructors: `public class Requirement(Guid ID)`

2. Namespace in file-scope: `namespace Activator.DomainDrivenDesigner.Domain.Project.Entities;`

3. Model description:
 - Execute `read_model_md_file("UML//Domain//DomainModels.md","Domain.Project","Requirement")`.

## Reference Domain Models:
- Execute `read_code_file("5-Support//Activator.DomainDrivenDesigner.Support.Core//Marks//EntityBase.cs")`.

## Do not generate Properties Inherited

## Output
Only output full source code of **Requirement.cs** in C# format, no other text. Use async/await correctly and Use ConfigureAwait(false) for each async call.