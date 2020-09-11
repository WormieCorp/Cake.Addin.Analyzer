namespace Cake.Addin.Analyzer.Rules
{
	using System;
	using System.Linq;
	using Cake.Addin.Analyzer.Constants;
	using Microsoft.CodeAnalysis;
	using Microsoft.CodeAnalysis.CSharp;
	using Microsoft.CodeAnalysis.CSharp.Syntax;
	using Microsoft.CodeAnalysis.Diagnostics;

	[DiagnosticAnalyzer(LanguageNames.CSharp)]
	public class AliasClassCategoryRule : BaseRule
	{
		private const string CakeAliasCategoryAttribute = "Cake.Core.Annotations.CakeAliasCategoryAttribute";

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
			=> context.RegisterSymbolAction(AnalyzeClassSymbol, SymbolKind.NamedType);

		private void AnalyzeClassSymbol(SymbolAnalysisContext obj)
		{
			if (!(obj.Symbol is INamedTypeSymbol symbol) || symbol.TypeKind != TypeKind.Class)
			{
				return;
			}

			var name = symbol.Name;

			if (!name.EndsWith("Alias", StringComparison.OrdinalIgnoreCase) &&
				!name.EndsWith("Aliases", StringComparison.OrdinalIgnoreCase))
			{
				return;
			}

			var attributes = symbol.GetAttributes();

			var hasAttribute = attributes.Any(a => HasExpectedAttribute(obj, a, CakeAliasCategoryAttribute));

			if (!hasAttribute)
			{
				var diagnostic = Diagnostic.Create(Rule, symbol.Locations[0], symbol.Name);
				obj.ReportDiagnostic(diagnostic);
			}
		}
	}
}
