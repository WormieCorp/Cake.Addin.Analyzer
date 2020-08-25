namespace CakeContrib.Analyzer.Rules
{
	using System.Collections.Generic;
	using System.Collections.Immutable;
	using System.Linq;
	using Microsoft.CodeAnalysis;
	using Microsoft.CodeAnalysis.CSharp.Syntax;
	using Microsoft.CodeAnalysis.Diagnostics;

	public abstract class BaseRule : DiagnosticAnalyzer
	{
		private readonly List<DiagnosticDescriptor> _additionalRules = new List<DiagnosticDescriptor>();

		protected BaseRule(
			string id,
			string titleName,
			string descriptionName,
			string messageFormatName,
			string category,
			DiagnosticSeverity severity = DiagnosticSeverity.Warning,
			bool isEnabledByDefault = true,
			params string[] customTags)
		{
			var rule = CreateRule(id, titleName, descriptionName, messageFormatName, category, severity, isEnabledByDefault, customTags);

			Rule = rule;
		}

		public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
		{
			get {
				var allRules = new[] { Rule }.Concat(AdditionalRules).ToArray();
				return ImmutableArray.Create(allRules);
			}
		}

		protected IReadOnlyList<DiagnosticDescriptor> AdditionalRules => this._additionalRules;
		protected DiagnosticDescriptor Rule { get; private set; }

		public override sealed void Initialize(AnalysisContext context)
		{
			ConfigureContext(context);
			RegisterActions(context);
		}

		protected static bool HasExpectedAttribute(SyntaxNodeAnalysisContext obj, AttributeSyntax attribute, string qualifiedTypeName)
		{
			var ti = obj.SemanticModel.GetTypeInfo(attribute);

			var metaType = obj.SemanticModel.Compilation.GetTypeByMetadataName(qualifiedTypeName);

			return ti.ConvertedType!.Equals(metaType, SymbolEqualityComparer.Default);
		}

		protected static bool HasExpectedParameter(SyntaxNodeAnalysisContext context, ParameterSyntax parameter, string qualifiedTypeName)
		{
			var ti = context.SemanticModel.GetTypeInfo(parameter.Type!);

			var metaType = context.SemanticModel.Compilation.GetTypeByMetadataName(qualifiedTypeName);

			return ti.ConvertedType!.Equals(metaType, SymbolEqualityComparer.Default);
		}

		protected void AddAdditionalRules(string id, string title, string description, string messageFormatName, string category, DiagnosticSeverity severity = DiagnosticSeverity.Warning, bool isEnabledByDefault = true, params string[] customTags)
		{
			var rule = CreateRule(id, title, description, messageFormatName, category, severity, isEnabledByDefault, customTags);

			this._additionalRules.Add(rule);
		}

		protected virtual void ConfigureContext(AnalysisContext context)
		{
			context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
			context.EnableConcurrentExecution();
		}

		protected abstract void RegisterActions(AnalysisContext context);

		private static DiagnosticDescriptor CreateRule(string id, string titleName, string descriptionName, string messageFormatName, string category, DiagnosticSeverity severity, bool isEnabledByDefault, string[] customTags)
		{
			var title = new LocalizableResourceString(titleName, Resources.ResourceManager, typeof(Resources));
			var description = new LocalizableResourceString(descriptionName, Resources.ResourceManager, typeof(Resources));
			var messageFormat = new LocalizableResourceString(messageFormatName, Resources.ResourceManager, typeof(Resources));

			var rule = new DiagnosticDescriptor(
				id,
				title,
				messageFormat,
				category,
				severity,
				isEnabledByDefault,
				description: description,
				helpLinkUri: $"https://github.com/AdmiringWorm/CakeContrib.Analyzer/wiki/Rules-{id}",
				customTags: customTags);
			return rule;
		}
	}
}
