namespace CakeContrib.Analyzer.Tests.CodeFixes
{
	using System;
	using System.Threading.Tasks;
	using CakeContrib.Analyzer.CodeFixes;
	using FluentAssertions;
	using Microsoft.CodeAnalysis.CodeFixes;
	using Microsoft.CodeAnalysis.CSharp;
	using Microsoft.CodeAnalysis.CSharp.Syntax;
	using NUnit.Framework;

	public class BaseCodefixProviderTests
	{
		[Test]
		public void ShouldReturnSimpleIdentifierNameWhenOnlyOneNamePartIsUsed()
		{
			var name = "Environment";
			var expected = SyntaxFactory.IdentifierName(name);

			var actual = MockedCodeFixProvider.BuildQualifiedName(name);

			actual.Should().BeOfType<IdentifierNameSyntax>();
			actual.IsEquivalentTo(expected).Should().BeTrue();
		}

		[Test]
		public void ShouldThrowExceptionWhenNoNamePartsIsAvailable()
		{
			Action act = () => MockedCodeFixProvider.BuildQualifiedName();
			var result = act.Should().Throw<ArgumentOutOfRangeException>()
				.And.Message.Should().StartWith("We need at least 1 name to build the correct same syntax");
		}

		private class MockedCodeFixProvider : BaseCodeFixProvider
		{
			public static new NameSyntax BuildQualifiedName(params string[] names)
				=> BaseCodeFixProvider.BuildQualifiedName(names);

			public override Task RegisterCodeFixesAsync(CodeFixContext context)
			{
				throw new System.NotImplementedException();
			}
		}
	}
}
