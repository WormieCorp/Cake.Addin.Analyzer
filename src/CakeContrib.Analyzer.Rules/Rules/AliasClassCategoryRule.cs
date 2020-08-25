namespace CakeContrib.Analyzer.Rules
{
	using System;
	using System.Linq;
	using CakeContrib.Analyzer.Constants;
	using Microsoft.CodeAnalysis;
	using Microsoft.CodeAnalysis.CSharp;
	using Microsoft.CodeAnalysis.CSharp.Syntax;
	using Microsoft.CodeAnalysis.Diagnostics;

	[DiagnosticAnalyzer(LanguageNames.CSharp)]
	public class AliasClassCategoryRule : BaseRule
	{
		public AliasClassCategoryRule()
			: base(
				Identifiers.AliasClassCategoryRule,
				nameof(Resources.AliasClassCategoryTitle),
				nameof(Resources.AliasClassCategoryDescription),
				nameof(Resources.AliasClassCategoryMessageFormat),
				Categories.Documentation,
				customTags: "Cake Build")
		{
		}

		protected override void RegisterActions(AnalysisContext context)
			=> context.RegisterSyntaxNodeAction(AnalyzeClassNode, SyntaxKind.ClassDeclaration);

		private void AnalyzeClassNode(SyntaxNodeAnalysisContext obj)
		{
			if (!(obj.Node is ClassDeclarationSyntax classDeclaration))
			{
				return;
			}

			var identifier = classDeclaration.Identifier;
			var identifierText = identifier.Text;
			if (!identifierText.EndsWith("Alias", StringComparison.OrdinalIgnoreCase) &&
				!identifierText.EndsWith("Aliases", StringComparison.OrdinalIgnoreCase))
			{
				return;
			}

			if (classDeclaration.AttributeLists.Any())
			{
				var attributes = classDeclaration.AttributeLists.SelectMany(al => al.Attributes);
				if (attributes.Any(a => HasExpectedAttribute(obj, a, "Cake.Core.Annotations.CakeAliasCategoryAttribute")))
				{
					return;
				}
			}

			var diagnostic = Diagnostic.Create(Rule, identifier.GetLocation(), identifierText);
			obj.ReportDiagnostic(diagnostic);
		}
	}
}
