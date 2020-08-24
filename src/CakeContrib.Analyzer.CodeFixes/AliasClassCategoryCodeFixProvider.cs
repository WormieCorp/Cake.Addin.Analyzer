namespace CakeContrib.Analyzer.CodeFixes
{
	using System;
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

	[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(AliasClassCategoryCodeFixProvider)), Shared]
	public class AliasClassCategoryCodeFixProvider : BaseCodeFixProvider
	{
		public AliasClassCategoryCodeFixProvider()
			: base(Identifiers.AliasClassCategoryRule)
		{
		}

		public override async Task RegisterCodeFixesAsync(CodeFixContext context)
		{
			var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false) ?? throw new ApplicationException("Unexpected null root was found");

			var diagnostic = context.Diagnostics.First();
			var diagnosticSpan = diagnostic.Location.SourceSpan;

			var parentToken = root.FindToken(diagnosticSpan.Start).Parent ?? throw new ApplicationException("No parent was found for the current diagnostic token!");

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
			var newAttributeList = SyntaxFactory.AttributeList().AddAttributes(newAttribute).WithAdditionalAnnotations(Formatter.Annotation);
			var newDeclaration = classDeclaration.AddAttributeLists(newAttributeList);

			var oldRoot = await document.GetSyntaxRootAsync(cancellationToken);
			if (oldRoot is null)
			{
				return document;
			}
			var newRoot = oldRoot.ReplaceNode(classDeclaration, newDeclaration);

			return document.WithSyntaxRoot(newRoot);
		}
	}
}