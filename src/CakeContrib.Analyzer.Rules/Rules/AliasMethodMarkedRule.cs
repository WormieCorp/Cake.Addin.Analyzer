namespace CakeContrib.Analyzer.Rules
{
	using System.Linq;
	using CakeContrib.Analyzer.Constants;
	using Microsoft.CodeAnalysis;
	using Microsoft.CodeAnalysis.CSharp;
	using Microsoft.CodeAnalysis.CSharp.Syntax;
	using Microsoft.CodeAnalysis.Diagnostics;

	[DiagnosticAnalyzer(LanguageNames.CSharp)]
	public sealed class AliasMethodMarkedRule : BaseRule
	{
		public AliasMethodMarkedRule()
			: base(
				  Identifiers.AliasMethodMarkedRule,
				  nameof(Resources.MethodMarkedTitle),
				  nameof(Resources.MethodMarkedDescription),
				  nameof(Resources.MethodMarkedMessageFormat),
				  Categories.Usage,
				  severity: DiagnosticSeverity.Error)
		{
		}

		protected override void RegisterActions(AnalysisContext context)
		{
			context.RegisterSyntaxNodeAction(AnalyzeMethodNode, SyntaxKind.MethodDeclaration);
		}

		private void AnalyzeMethodNode(SyntaxNodeAnalysisContext obj)
		{
			if (!(obj.Node is MethodDeclarationSyntax methodDecl))
			{
				return;
			}

			var firstParameter = methodDecl.ParameterList.Parameters.FirstOrDefault();

			if (firstParameter is null || !IsIContextType(obj, firstParameter) || !firstParameter.Modifiers.Any(SyntaxKind.ThisKeyword))
			{
				return;
			}

			var attributes = methodDecl.AttributeLists.SelectMany(a => a.Attributes);
			if (attributes.Any(a => HasExpectedAttribute(obj, a)))
			{
				return;
			}

			var diagnostic = Diagnostic.Create(Rule, methodDecl.Identifier.GetLocation(), methodDecl.Identifier.Text);
			obj.ReportDiagnostic(diagnostic);
		}

		private bool HasExpectedAttribute(SyntaxNodeAnalysisContext obj, AttributeSyntax attribute)
		{
			var ti = obj.SemanticModel.GetTypeInfo(attribute);

			var metaType = obj.SemanticModel.Compilation.GetTypeByMetadataName("Cake.Core.Annotations.CakeMethodAliasAttribute");

			return ti.ConvertedType!.Equals(metaType, SymbolEqualityComparer.Default);
		}

		private bool IsIContextType(SyntaxNodeAnalysisContext obj, ParameterSyntax parameter)
		{
			var ti = obj.SemanticModel.GetTypeInfo(parameter.Type!);

			var metaType = obj.SemanticModel.Compilation.GetTypeByMetadataName("Cake.Core.ICakeContext");

			return ti.ConvertedType!.Equals(metaType, SymbolEqualityComparer.Default);
		}
	}
}
