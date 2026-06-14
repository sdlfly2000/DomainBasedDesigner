namespace Activator.DomainDrivenDesigner.Infrastructure.AI.Model;

public record SemanticAnalysisRelationshipResult(string noun, string[] modifiers);

public record SemanticAnalysisResult(string[] verbs, string[] nouns, string[] modifiers, SemanticAnalysisRelationshipResult[] relationships);

