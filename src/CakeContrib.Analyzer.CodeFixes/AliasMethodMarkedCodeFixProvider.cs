namespace CakeContrib.Analyzer.CodeFixes
{
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
			var diagnostic = context.Diagnostics.First();

			var declaration = await FindDeclarationAsync<MethodDeclarationSyntax>(context, diagnostic).ConfigureAwait(false);

			context.RegisterCodeFix(
				CodeAction.Create(
					title: CodeFixResources.AliasMethodMarkedTitle,
					createChangedDocument: c => AddCakeMethodAliasAsync(context.Document, declaration, c),
					equivalenceKey: nameof(CodeFixResources.AliasMethodMarkedTitle)),
				diagnostic);

			if (declaration.ParameterList.Parameters.Count == 1)
			{
				context.RegisterCodeFix(
					CodeAction.Create(
						title: CodeFixResources.AliasMethodPropertyMarkedTitle,
						createChangedDocument: c => AddCakePropertyAliasAsync(context.Document, declaration, c),
						equivalenceKey: nameof(CodeFixResources.AliasMethodPropertyMarkedTitle)),
					diagnostic);
			}
		}

		private static Task<Document> AddCakeMethodAliasAsync(Document document, MethodDeclarationSyntax methodDeclaration, CancellationToken cancellationToken)
		{
			var qualifiedName = BuildQualifiedName("Cake", "Core", "Annotations", "CakeMethodAliasAttribute");
			return AddNewAttribute(document, methodDeclaration, qualifiedName, cancellationToken);
		}

		private static Task<Document> AddCakePropertyAliasAsync(Document document, MethodDeclarationSyntax methodDeclaration, CancellationToken cancellationToken)
		{
			var qualifiedName = BuildQualifiedName("Cake", "Core", "Annotations", "CakePropertyAliasAttribute");
			return AddNewAttribute(document, methodDeclaration, qualifiedName, cancellationToken);
		}

		private static async Task<Document> AddNewAttribute(Document document, MethodDeclarationSyntax methodDeclaration, NameSyntax qualifiedName, CancellationToken cancellationToken)
		{
			var newAttribute = SyntaxFactory.Attribute(qualifiedName);
			var newAttributeList = SyntaxFactory.AttributeList().AddAttributes(newAttribute).WithAdditionalAnnotations(Formatter.Annotation);
			var newDeclaration = methodDeclaration.AddAttributeLists(newAttributeList);

			var oldRoot = await document.GetSyntaxRootAsync(cancellationToken);
			var newRoot = oldRoot!.ReplaceNode(methodDeclaration, newDeclaration);

			return document.WithSyntaxRoot(newRoot);
		}
	}
}
