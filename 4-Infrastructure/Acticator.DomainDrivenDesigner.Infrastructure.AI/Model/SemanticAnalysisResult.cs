namespace Activator.DomainDrivenDesigner.Infrastructure.AI.Model;

public record SemanticAnalysisResult(string[] verbs, string[] nouns, string[] modifiers, (string, string[])[] relationship);

