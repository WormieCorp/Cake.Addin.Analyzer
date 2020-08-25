namespace CakeContrib.Analyzer.Test
{
	using System.Collections.Immutable;
	using System.Threading;
	using System.Threading.Tasks;
	using Microsoft.CodeAnalysis;
	using Microsoft.CodeAnalysis.CodeFixes;
	using Microsoft.CodeAnalysis.CSharp.Testing;
	using Microsoft.CodeAnalysis.Diagnostics;
	using Microsoft.CodeAnalysis.Testing;
	using Microsoft.CodeAnalysis.Testing.Verifiers;

	public static partial class CSharpCodeFixVerifier<TAnalyzer, TCodeFix>
		where TAnalyzer : DiagnosticAnalyzer, new()
		where TCodeFix : CodeFixProvider, new()
	{
		/// <inheritdoc cref="CodeFixVerifier{TAnalyzer, TCodeFix, TTest, TVerifier}.Diagnostic()"/>
		public static DiagnosticResult Diagnostic()
			=> CSharpCodeFixVerifier<TAnalyzer, TCodeFix, NUnitVerifier>.Diagnostic();

		/// <inheritdoc cref="CodeFixVerifier{TAnalyzer, TCodeFix, TTest, TVerifier}.Diagnostic(string)"/>
		public static DiagnosticResult Diagnostic(string diagnosticId)
			=> CSharpCodeFixVerifier<TAnalyzer, TCodeFix, NUnitVerifier>.Diagnostic(diagnosticId);

		/// <inheritdoc cref="CodeFixVerifier{TAnalyzer, TCodeFix, TTest, TVerifier}.Diagnostic(DiagnosticDescriptor)"/>
		public static DiagnosticResult Diagnostic(DiagnosticDescriptor descriptor)
			=> CSharpCodeFixVerifier<TAnalyzer, TCodeFix, NUnitVerifier>.Diagnostic(descriptor);

		/// <inheritdoc cref="CodeFixVerifier{TAnalyzer, TCodeFix, TTest, TVerifier}.VerifyAnalyzerAsync(string, DiagnosticResult[])"/>
		public static async Task VerifyAnalyzerAsync(string source, params DiagnosticResult[] expected)
		{
			var test = new Test {
				TestCode = source,
				ReferenceAssemblies =
					ReferenceAssemblies.Default.AddPackages(
						ImmutableArray.Create(
							new PackageIdentity("Cake.Core", "0.38.4"),
							new PackageIdentity("Cake.Common", "0.38.4"))),
			};

			test.ExpectedDiagnostics.AddRange(expected);
			await test.RunAsync(CancellationToken.None);
		}

		/// <inheritdoc cref="CodeFixVerifier{TAnalyzer, TCodeFix, TTest, TVerifier}.VerifyCodeFixAsync(string, string)"/>
		public static async Task VerifyCodeFixAsync(string source, string fixedSource)
			=> await VerifyCodeFixAsync(source, DiagnosticResult.EmptyDiagnosticResults, fixedSource);

		/// <inheritdoc cref="CodeFixVerifier{TAnalyzer, TCodeFix, TTest, TVerifier}.VerifyCodeFixAsync(string, DiagnosticResult, string)"/>
		public static async Task VerifyCodeFixAsync(string source, DiagnosticResult expected, string fixedSource, string codefixEquivalenceKey = null)
			=> await VerifyCodeFixAsync(source, new[] { expected }, fixedSource, codefixEquivalenceKey);

		/// <inheritdoc cref="CodeFixVerifier{TAnalyzer, TCodeFix, TTest, TVerifier}.VerifyCodeFixAsync(string, DiagnosticResult[], string)"/>
		public static async Task VerifyCodeFixAsync(string source, DiagnosticResult[] expected, string fixedSource, string codefixEquivalenceKey = null)
		{
			var test = new Test {
				TestCode = source,
				FixedCode = fixedSource,
				ReferenceAssemblies =
					ReferenceAssemblies.Default.AddPackages(
						ImmutableArray.Create(
							new PackageIdentity("Cake.Core", "0.38.4"),
							new PackageIdentity("Cake.Common", "0.38.4"))),
			};

			if (!string.IsNullOrEmpty(codefixEquivalenceKey))
			{
				test.CodeActionEquivalenceKey = codefixEquivalenceKey;
			}

			test.ExpectedDiagnostics.AddRange(expected);
			await test.RunAsync(CancellationToken.None);
		}
	}
}
