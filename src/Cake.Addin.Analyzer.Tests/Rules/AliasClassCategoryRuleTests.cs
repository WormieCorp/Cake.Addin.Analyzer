namespace Cake.Addin.Analyzer.Test.Rules
{
	using System.Threading.Tasks;
	using Cake.Addin.Analyzer.Constants;
	using Cake.Addin.Analyzer.Tests;
	using NUnit.Framework;
	using VerifyCS = CSharpCodeFixVerifier<
		Analyzer.Rules.AliasClassCategoryRule,
		CodeFixes.AliasClassCategoryCodeFixProvider>;

	public class AliasClassCategoryRuleTests
	{
		[Category("Code Fix")]
		[TestCase(TestTemplates.AliasClassCategory.DoNotHaveCakeAliasCategoryAliased, TestTemplates.AliasClassCategory.HaveCakeAliasCategoryAliased, TestName = "ShouldFixToAliasedNameWhenReferenced")]
		[TestCase(TestTemplates.AliasClassCategory.DoNotHaveCakeAliasCategoryQualified, TestTemplates.AliasClassCategory.HaveCakeAliasCategoryQualified, TestName = "ShouldFixToQualifiedNameWhenNoReference")]
		[TestCase(TestTemplates.AliasClassCategory.DoNotHaveCakeAliasCategorySimplified, TestTemplates.AliasClassCategory.HaveCakeAliasCategorySimplified, TestName = "ShouldFixToSimplifiedNameWhenReferenced")]
		public async Task ShouldApplyCodeFix(string test, string fix)
		{
			var expected = VerifyCS.Diagnostic(Identifiers.AliasClassCategoryRule)
				.WithLocation(0).WithArguments("CakeAddinAliases");
			await VerifyCS.VerifyCodeFixAsync(test, expected, fix);
		}

		[Category("Analyzing")]
		[TestCase(TestTemplates.AliasClassCategory.HaveCakeAliasCategoryAliased, TestName = "UsingAliasShouldBeValid")]
		[TestCase(TestTemplates.AliasClassCategory.HaveCakeAliasCategoryQualified, TestName = "UsingQualifiedShouldBeValid")]
		[TestCase(TestTemplates.AliasClassCategory.HaveCakeAliasCategorySimplified, TestName = "UsingSimplifiedShouldBeValid")]
		[TestCase(TestTemplates.EmptyGeneralClass, TestName = "NonAliasClassShouldBeValid")]
		public async Task ShouldBeValid(string test)
			=> await VerifyCS.VerifyAnalyzerAsync(test);
	}
}
