namespace CakeContrib.Analyzer.Rules
{
	using System.Linq;
	using CakeContrib.Analyzer.Constants;
	using Microsoft.CodeAnalysis;
	using Microsoft.CodeAnalysis.Diagnostics;

	[DiagnosticAnalyzer(LanguageNames.CSharp)]
	public class UpperCaseRule : BaseRule
	{
		static UpperCaseRule()
		{
			SetRule(
				  Identifiers.UpperCaseRule,
				  nameof(Resources.UpperCaseRuleTitle),
				  nameof(Resources.UpperCaseRuleDescription),
				  nameof(Resources.UpperCaseRuleMessageFormat),
				  Categories.Naming);
		}

		protected override void RegisterActions(AnalysisContext context)
			=> context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.NamedType);

		private static void AnalyzeSymbol(SymbolAnalysisContext context)
		{
			// TODO: Replace the following code with your own analysis, generating Diagnostic objects for any issues you find
			var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;

			// Find just those named type symbols with names containing lowercase letters.
			if (namedTypeSymbol.Name.ToCharArray().Any(char.IsLower))
			{
				// For all such symbols, produce a diagnostic.
				var diagnostic = Diagnostic.Create(Rule, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);

				context.ReportDiagnostic(diagnostic);
			}
		}
	}
}
