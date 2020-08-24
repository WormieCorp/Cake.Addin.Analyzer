namespace CakeContrib.Analyzer.CodeFixes
{
	using System;
	using System.Collections.Immutable;
	using System.Composition;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;
	using CakeContrib.Analyzer.Constants;
	using Microsoft.CodeAnalysis;
	using Microsoft.CodeAnalysis.CodeActions;
	using Microsoft.CodeAnalysis.CodeFixes;
	using Microsoft.CodeAnalysis.CSharp;
	using Microsoft.CodeAnalysis.CSharp.Syntax;
	using Microsoft.CodeAnalysis.Formatting;
	using Microsoft.CodeAnalysis.Simplification;

	[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(AliasClassCategoryCodeFixProvider)), Shared]
	public class AliasClassCategoryCodeFixProvider : CodeFixProvider
	{
		public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(Identifiers.AliasClassCategoryRule);

		public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

		public override async Task RegisterCodeFixesAsync(CodeFixContext context)
		{
			var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

			if (root is null)
			{
				return;
			}

			var diagnostic = context.Diagnostics.First();
			var diagnosticSpan = diagnostic.Location.SourceSpan;

			var parentToken = root.FindToken(diagnosticSpan.Start).Parent;
			if (parentToken is null)
			{
				return;
			}

			var declaration = parentToken.AncestorsAndSelf().OfType<ClassDeclarationSyntax>().First();

			context.RegisterCodeFix(
				CodeAction.Create(
					title: CodeFixResources.AliasClassCategoryTitle,
					createChangedDocument: c => AddCakeAliasCategoryAsync(context.Document, declaration, c),
					equivalenceKey: nameof(CodeFixResources.AliasClassCategoryTitle)),
				diagnostic);
		}

		private async Task<Document> AddCakeAliasCategoryAsync(Document document, ClassDeclarationSyntax classDeclaration, CancellationToken cancellationToken)
		{
			var qualifiedName = BuildQualifiedName("Cake", "Core", "Annotations", "CakeAliasCategoryAttribute");

			var argument = SyntaxFactory.AttributeArgument(
				SyntaxFactory.LiteralExpression(
					SyntaxKind.StringLiteralExpression,
					SyntaxFactory.Literal("REPLACE_ME")));
			var newAttribute = SyntaxFactory.Attribute(qualifiedName)
				.AddArgumentListArguments(argument);
			var newAttributeList = SyntaxFactory.AttributeList().AddAttributes(newAttribute);
			var newDeclaration = classDeclaration.AddAttributeLists(newAttributeList);

			var oldRoot = await document.GetSyntaxRootAsync(cancellationToken);
			if (oldRoot is null)
			{
				return document;
			}
			var newRoot = oldRoot.ReplaceNode(classDeclaration, newDeclaration);

			return document.WithSyntaxRoot(newRoot);
		}

		private static NameSyntax BuildQualifiedName(params string[] names)
		{
			if (names.Length == 0)
			{
				throw new ArgumentOutOfRangeException(nameof(names), "We need at least 1 name to build the correct same syntax");
			}
			else if (names.Length == 1)
			{
				return SyntaxFactory.IdentifierName(names[0]);
			}

			NameSyntax? qualifiedName = default;

			foreach (var name in names)
			{
				var identifierName = SyntaxFactory.IdentifierName(name);
				if (qualifiedName is null)
				{
					qualifiedName = identifierName;
				}
				else
				{
					qualifiedName = SyntaxFactory.QualifiedName(qualifiedName, identifierName)
						.WithAdditionalAnnotations(Simplifier.Annotation);
				}
			}

#pragma warning disable CS8603 // Possible null reference return.
			return qualifiedName;
#pragma warning restore CS8603 // Possible null reference return.
		}
	}
}
