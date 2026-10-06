## Generate complete C# code 
Target File: **3-Domain//Activator.DomainDrivenDesigner.Domain//BusinessModel//Entities//BusinessModel.cs**

## Format:
- **Formatting Style:** Strictly use Allman style (opening braces `{` must always be placed on a new line for classes, methods, and control blocks).

- Write a C# **BusinessModel** class

    ```csharp
    namespace Activator.DomainDrivenDesigner.Domain.BusinessModel.Entities;

    public class BusinessModel(Guid ID) : EntityBase(ID)
    {
        // Your Full Code Implementation (Allman Style including Using statements)
    }
    ```

## Rules:
1. Class: **BusinessModel**, Inherits **EntityBase**

2. Namespace in file-scope: `namespace Activator.DomainDrivenDesigner.Domain.BusinessModel.Entities;`

3. Model description:
- Execute `read_model_md_file("UML//Domain//DomainModels.md","Domain.BusinessModel","BusinessModel")`.

## Reference Domain Models:
- Execute `read_code_file("5-Support//Activator.DomainDrivenDesigner.Support.Core//Marks//EntityBase.cs")`.

## Do not generate Properties Inherited:

## Output
Only output full source code of **BusinessModel.cs** in C# format, no other text. Use async/await correctly and Use ConfigureAwait(false) for each async call.