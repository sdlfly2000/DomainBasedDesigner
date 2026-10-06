## Generate complete C# code 
Target File: **3-Domain//Activator.DomainDrivenDesigner.Domain//Project//Entities//Project.cs**

## Format:
- **Formatting Style:** Strictly use Allman style (opening braces `{` must always be placed on a new line for classes, methods, and control blocks).

- Write a C# **Project** class

    ```csharp
    namespace Activator.DomainDrivenDesigner.Domain.Project.Entities;

    public class Project(Guid ID) : EntityBase(ID)
    {
        // Your Full Code Implementation (Allman Style including Using statements)
    }
    ```

## Rules:
1. Class: **Project**, Inherits **EntityBase**

2. Namespace in file-scope: `namespace Activator.DomainDrivenDesigner.Domain.Project.Entities;`

3. Model description:
 - Execute `read_model_md_file("UML//Domain//DomainModels.md","Domain.Project","Project")`.

## Public Constructors: 
- `public Project(Guid ID, string ProjectName)`

## Static Methods:
- Method Signature: `public static Project Create(string ProjectName, string ProjectDescription, string? BaseDirectory = null)`
  Logic: Create a new Project with ProjectName, and mapping Description = ProjectDescription ...

## Reference Domain Models:
- Execute `read_code_file("5-Support//Activator.DomainDrivenDesigner.Support.Core//Marks//EntityBase.cs")`.

## Do not generate Properties Inherited:

## Output
Only output full source code of **Project.cs** in C# format, no other text. Use async/await correctly and Use ConfigureAwait(false) for each async call.