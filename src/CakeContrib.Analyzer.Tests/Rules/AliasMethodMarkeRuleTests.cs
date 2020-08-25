namespace CakeContrib.Analyzer.Tests.Rules
{
	using System.Threading.Tasks;
	using CakeContrib.Analyzer.Constants;
	using Microsoft.CodeAnalysis.Testing;
	using NUnit.Framework;
	using VerifyCS = Test.CSharpCodeFixVerifier<
		Analyzer.Rules.AliasMethodMarkedRule,
		Analyzer.CodeFixes.AliasMethodMarkedCodeFixProvider>;

	public class AliasMethodMarkeRuleTests
	{
		/*[Category("Code Fix")]
		public async Task ShouldApplyCodeFix(string test, string fix)
		{
			var expected = VerifyCS.Diagnostic(Identifiers.AliasClassCategoryRule)
				.WithLocation(0).WithArguments("CakeAddinAliases");
			await VerifyCS.VerifyCodeFixAsync(test, expected, fix);
		}*/

		[Category("Analyzing")]
		[TestCase(TestTemplates.AliasMethodMarked_DoNotHaveCakeMethodAliasAliased, TestTemplates.AliasMethodMarked_HaveCakeMethodAliasAliased, TestName = "NotUsingAliasedShouldNotBeValid")]
		[TestCase(TestTemplates.AliasMethodMarked_DoNotHaveCakeMethodAliasQualified, TestTemplates.AliasMethodMarked_HaveCakeMethodAliasQualified, TestName = "NotUsingQualifiedShouldNotBeValid")]
		[TestCase(TestTemplates.AliasMethodMarked_DoNotHaveCakeMethodAliasSimplified, TestTemplates.AliasMethodMarked_HaveCakeMethodAliasSimplified, TestName = "NotUsingSimplifiedShouldNotBeValid")]
		public async Task ShouldBeCorrectlyFixed(string test, string fixtest)
		{
			var expected = VerifyCS.Diagnostic(Identifiers.AliasMethodMarkedRule)
				.WithLocation(0).WithArguments("MyAwesomeAlias");

			await VerifyCS.VerifyCodeFixAsync(test, expected, fixtest);
		}

		[Category("Analyzing")]
		[TestCase(TestTemplates.AliasMethodMarked_HaveCakeMethodAliasAliased, TestName = "UsingAliasedShouldBeValid")]
		[TestCase(TestTemplates.AliasMethodMarked_HaveCakeMethodAliasQualified, TestName = "UsingQualifiedShouldBeValid")]
		[TestCase(TestTemplates.AliasMethodMarked_HaveCakeMethodAliasSimplified, TestName = "UsingSimplifiedShouldBeValid")]
		[TestCase(TestTemplates.EmptyGeneralClass, TestName = "NonAliasClassShouldBeValid")]
		[TestCase(TestTemplates.AliasMethodMarked_IsNotAAliasMethod, TestName = "NonAliasMethodShouldBeValid")]
		[TestCase(TestTemplates.AliasMethodMarked_HaveContextButNotThis, TestName = "ContextWithoutThisKeywoardShouldBeValid")]
		public async Task ShouldBeValid(string test)
			=> await VerifyCS.VerifyAnalyzerAsync(test);

		[Category("Analyzing")]
		[Test]
		public async Task ShouldBeValidWhenParameterIsUnreferenced()
		{
			var test = @"
namespace CakeAddinTest
{
	public static class CakeAddin
	{
		public static void MyAliasMethod(this {|#0:ICakeContext|} context, string ohyes)
		{
			ohyes = ohyes;
		}
	}
}";

			var expected = DiagnosticResult.CompilerError("CS0246").WithLocation(0).WithArguments("ICakeContext");

			await VerifyCS.VerifyAnalyzerAsync(test, expected);
		}

		[Category("Analyzing")]
		[Test]
		public async Task ShouldNotBeValidWhenParameterIsUnreferenced()
		{
			var test = @"
using Cake.Core;

namespace CakeAddinTest
{
	public static class CakeAddin
	{
		[{|#1:CakeMethodAlias|}]
		public static void {|#0:MyAliasMethod|}(this ICakeContext context, string ohyes)
		{
			ohyes = ohyes;
		}
	}
}";
			var expectedDiagnosics = new[] {
				DiagnosticResult.CompilerError("CS0246").WithLocation(1).WithArguments("CakeMethodAlias"),
				DiagnosticResult.CompilerError("CS0246").WithLocation(1).WithArguments("CakeMethodAliasAttribute"),
				VerifyCS.Diagnostic(Identifiers.AliasMethodMarkedRule).WithLocation(0).WithArguments("MyAliasMethod"),
			};

			await VerifyCS.VerifyAnalyzerAsync(test, expectedDiagnosics);
		}
	}
}
