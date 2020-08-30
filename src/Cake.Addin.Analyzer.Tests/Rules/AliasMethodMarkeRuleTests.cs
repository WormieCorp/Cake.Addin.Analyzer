namespace Cake.Addin.Analyzer.Tests.Rules
{
	using System.Threading.Tasks;
	using Cake.Addin.Analyzer.Constants;
	using Microsoft.CodeAnalysis.Testing;
	using NUnit.Framework;
	using VerifyCS = Test.CSharpCodeFixVerifier<
		Analyzer.Rules.AliasMethodMarkedRule,
		Analyzer.CodeFixes.AliasMethodMarkedCodeFixProvider>;

	public class AliasMethodMarkeRuleTests
	{
		[Category("Code Fix")]
		[TestCase(TestTemplates.AliasMethodMarked.DoNotHaveCakeMethodAliasAliased, TestTemplates.AliasMethodMarked.HaveCakeMethodAliasAliased, TestName = "NotUsingAliasedShouldNotBeValid")]
		[TestCase(TestTemplates.AliasMethodMarked.DoNotHaveCakeMethodAliasQualified, TestTemplates.AliasMethodMarked.HaveCakeMethodAliasQualified, TestName = "NotUsingQualifiedShouldNotBeValid")]
		[TestCase(TestTemplates.AliasMethodMarked.DoNotHaveCakeMethodAliasSimplified, TestTemplates.AliasMethodMarked.HaveCakeMethodAliasSimplified, TestName = "NotUsingSimplifiedShouldNotBeValid")]
		[TestCase(TestTemplates.Methods.DoNotHaveCakeAliasOnSingleCakeContextParameter, TestTemplates.AliasMethodMarked.HaveCakeAliasOnSingleCakeContextParameter, TestName = "ShouldFixWhenUsingSingleParameter")]
		public async Task ShouldApplyMethodCodeFix(string test, string fixtest)
		{
			var expected = VerifyCS.Diagnostic(Identifiers.AliasMethodMarkedRule)
				.WithLocation(0).WithArguments("MyAwesomeAlias");

			await VerifyCS.VerifyCodeFixAsync(test, expected, fixtest, nameof(Analyzer.CodeFixes.CodeFixResources.AliasMethodMarkedTitle));
		}

		[Category("Code Fix")]
		[TestCase(TestTemplates.AliasMethodMarked.DoNotHaveCakePropertyAliasAliased, TestTemplates.AliasMethodMarked.HaveCakePropertyAliasAliased, TestName = "NotUsingPropertyAliasedShouldNotBeValid")]
		[TestCase(TestTemplates.AliasMethodMarked.DoNotHaveCakePropertyAliasQualified, TestTemplates.AliasMethodMarked.HaveCakePropertyAliasQualified, TestName = "NotUsingPropertyQualifiedShouldNotBeValid")]
		[TestCase(TestTemplates.AliasMethodMarked.DoNotHaveCakePropertyAliasSimplified, TestTemplates.AliasMethodMarked.HaveCakePropertyAliasSimplified, TestName = "NotUsingPropertySimplifiedShouldNotBeValid")]
		public async Task ShouldApplyPropertyCodeFix(string test, string fixtest)
		{
			var expected = VerifyCS.Diagnostic(Identifiers.AliasMethodMarkedRule)
				.WithLocation(0).WithArguments("MyAwesomeAlias");

			await VerifyCS.VerifyCodeFixAsync(test, expected, fixtest, nameof(Analyzer.CodeFixes.CodeFixResources.AliasMethodPropertyMarkedTitle));
		}

		[Category("Analyzing")]
		[TestCase(TestTemplates.EmptyGeneralClass, TestName = "NonAliasClassShouldBeValid")]
		[TestCase(TestTemplates.AliasMethodMarked.HaveCakeMethodAliasAliased, TestName = "UsingAliasedShouldBeValid")]
		[TestCase(TestTemplates.AliasMethodMarked.HaveCakeMethodAliasQualified, TestName = "UsingQualifiedShouldBeValid")]
		[TestCase(TestTemplates.AliasMethodMarked.HaveCakeMethodAliasSimplified, TestName = "UsingSimplifiedShouldBeValid")]
		[TestCase(TestTemplates.AliasMethodMarked.HaveCakePropertyAliasAliased, TestName = "UsingAliasedPropertyShouldBeValid")]
		[TestCase(TestTemplates.AliasMethodMarked.HaveCakePropertyAliasQualified, TestName = "UsingQualifiedPropertyShouldBeValid")]
		[TestCase(TestTemplates.AliasMethodMarked.HaveCakePropertyAliasSimplified, TestName = "UsingSimplifiedPropertyShouldBeValid")]
		[TestCase(TestTemplates.AliasMethodMarked.IsNotAAliasMethod, TestName = "NonAliasMethodShouldBeValid")]
		[TestCase(TestTemplates.AliasMethodMarked.HaveContextButNotThis, TestName = "ContextWithoutThisKeywoardShouldBeValid")]
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
