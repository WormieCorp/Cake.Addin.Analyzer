namespace CakeContrib.Analyzer.Test.Rules
{
	using System.Threading.Tasks;
	using CakeContrib.Analyzer.Constants;
	using NUnit.Framework;
	using VerifyCS = CSharpCodeFixVerifier<
		Analyzer.Rules.AliasClassCategoryRule,
		CodeFixes.AliasClassCategoryCodeFixProvider>;

	public class AliasClassCategoryRuleTests
	{
		[Test]
		public async Task ShouldBeValidWhenAttributeIsUsedAsShortName()
		{
			var test = @"
using Cake.Core.Annotations;

namespace CakeAddin
{
	[CakeAliasCategory(""SomeCategory"")]
	public static class CakeAddinAliases
	{
	}
}";
			await VerifyCS.VerifyAnalyzerAsync(test);
		}

		[Test]
		public async Task ShouldBeValidWhenAttributeIsUsedFullyQualified()
		{
			var test = @"
namespace CakeAddin
{
	[Cake.Core.Annotations.CakeAliasCategory(""SomeCategory"")]
	public static class CakeAddinAliases
	{
	}
}";
			await VerifyCS.VerifyAnalyzerAsync(test);
		}

		[Test]
		public async Task ShouldBeValidWhenAttributeIsUsedWithUsingAlias()
		{
			var test = @"
using Category = Cake.Core.Annotations.CakeAliasCategoryAttribute;

namespace CakeAddin
{
	[Category(""SomeCategory"")]
	public static class CakeAddinAliases
	{
	}
}";

			await VerifyCS.VerifyAnalyzerAsync(test);
		}

		[Test]
		public async Task ShouldNotTriggerDiagnosticOnNonAliasClass()
		{
			var test = @"
namespace CakeAddin
{
	public class TestClass1
	{
	}
}";

			await VerifyCS.VerifyAnalyzerAsync(test);
		}

		[Test]
		public async Task ShouldReportWarningWhenNoCategoryAttributeIsUsedAnAliasClass()
		{
			var test = @"
namespace CakeAddin
{
    public static class {|#0:CakeAddinAliases|}
    {
    }
}";
			var fixtest = @"
namespace CakeAddin
{
    [Cake.Core.Annotations.CakeAliasCategory(""REPLACE_ME"")]
    public static class CakeAddinAliases
    {
    }
}";

			var expected = VerifyCS.Diagnostic(Identifiers.AliasClassCategoryRule)
				.WithLocation(0).WithArguments("CakeAddinAliases");
			await VerifyCS.VerifyCodeFixAsync(test, expected, fixtest);
		}

		[Test]
		public async Task ShouldReportAndFixWarningWhenNoCategoryAttributeIsUsedWithShortName()
		{
			var test = @"
using Cake.Core.Annotations;

namespace CakeAddin
{
    public static class {|#0:CakeAddinAliases|}
    {
    }
}";
			var fixtest = @"
using Cake.Core.Annotations;

namespace CakeAddin
{
    [CakeAliasCategory(""REPLACE_ME"")]
    public static class CakeAddinAliases
    {
    }
}";

			var expected = VerifyCS.Diagnostic(Identifiers.AliasClassCategoryRule)
				.WithLocation(0).WithArguments("CakeAddinAliases");
			await VerifyCS.VerifyCodeFixAsync(test, expected, fixtest);
		}
	}
}
