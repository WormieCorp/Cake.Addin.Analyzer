namespace CakeContrib.Analyzer.Test.Rules
{
	using System.Threading.Tasks;
	using CakeContrib.Analyzer.Constants;
	using CakeContrib.Analyzer.Tests;
	using NUnit.Framework;
	using VerifyCS = CSharpCodeFixVerifier<
		Analyzer.Rules.AliasClassCategoryRule,
		CodeFixes.AliasClassCategoryCodeFixProvider>;

	public class AliasClassCategoryRuleTests
	{
		[Category("Code Fix")]
		[TestCase(TestTemplates.AliasClassCategory_DoNotHaveCakeAliasCategoryAliased, TestTemplates.AliasClassCategory_HaveCakeAliasCategoryAliased, TestName = "ShouldFixToAliasedNameWhenReferenced")]
		[TestCase(TestTemplates.AliasClassCategory_DoNotHaveCakeAliasCategoryQualified, TestTemplates.AliasClassCategory_HaveCakeAliasCategoryQualified, TestName = "ShouldFixToQualifiedNameWhenNoReference")]
		[TestCase(TestTemplates.AliasClassCategory_DoNotHaveCakeAliasCategorySimplified, TestTemplates.AliasClassCategory_HaveCakeAliasCategorySimplified, TestName = "ShouldFixToSimplifiedNameWhenReferenced")]
		public async Task ShouldApplyCodeFix(string test, string fix)
		{
			var expected = VerifyCS.Diagnostic(Identifiers.AliasClassCategoryRule)
				.WithLocation(0).WithArguments("CakeAddinAliases");
			await VerifyCS.VerifyCodeFixAsync(test, expected, fix);
		}

		[Category("Analyzing")]
		[TestCase(TestTemplates.AliasClassCategory_HaveCakeAliasCategoryAliased, TestName = "UsingAliasShouldBeValid")]
		[TestCase(TestTemplates.AliasClassCategory_HaveCakeAliasCategoryQualified, TestName = "UsingQualifiedShouldBeValid")]
		[TestCase(TestTemplates.AliasClassCategory_HaveCakeAliasCategorySimplified, TestName = "UsingSimplifiedShouldBeValid")]
		[TestCase(TestTemplates.EmptyGeneralClass, TestName = "NonAliasClassShouldBeValid")]
		public async Task ShouldBeValid(string test)
			=> await VerifyCS.VerifyAnalyzerAsync(test);
	}
}
