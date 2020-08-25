namespace CakeContrib.Analyzer.CodeFixes
{
	using System;
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

	[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(AliasMethodMarkedCodeFixProvider))]
	public sealed class AliasMethodMarkedCodeFixProvider : BaseCodeFixProvider
	{
		public AliasMethodMarkedCodeFixProvider()
			: base(Identifiers.AliasMethodMarkedRule)
		{
		}

		public override async Task RegisterCodeFixesAsync(CodeFixContext context)
		{
			var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false) ?? throw new ApplicationException("Unexpected null root was found");

			var diagnostic = context.Diagnostics.First();
			var diagnosticSpan = diagnostic.Location.SourceSpan;

			var parentToken = root.FindToken(diagnosticSpan.Start).Parent ?? throw new ApplicationException("No parent was found for the current diagnostic token!");

			var declaration = parentToken.AncestorsAndSelf().OfType<MethodDeclarationSyntax>().First();

			context.RegisterCodeFix(
				CodeAction.Create(
					title: CodeFixResources.AliasMethodMarkedTitle,
					createChangedDocument: c => AddCakeMethodAliasAsync(context.Document, declaration, c),
					equivalenceKey: nameof(CodeFixResources.AliasMethodMarkedTitle)),
				diagnostic);
		}

		private static async Task<Document> AddCakeMethodAliasAsync(Document document, MethodDeclarationSyntax methodDeclaration, CancellationToken cancellationToken)
		{
			var qualifiedName = BuildQualifiedName("Cake", "Core", "Annotations", "CakeMethodAliasAttribute");

			var newAttribute = SyntaxFactory.Attribute(qualifiedName);
			var newAttributeList = SyntaxFactory.AttributeList().AddAttributes(newAttribute).WithAdditionalAnnotations(Formatter.Annotation);
			var newDeclaration = methodDeclaration.AddAttributeLists(newAttributeList);

			var oldRoot = await document.GetSyntaxRootAsync(cancellationToken);
			if (oldRoot is null)
			{
				return document;
			}
			var newRoot = oldRoot.ReplaceNode(methodDeclaration, newDeclaration);

			return document.WithSyntaxRoot(newRoot);
		}
	}
}
