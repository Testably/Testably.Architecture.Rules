using FluentAssertions;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Filters;

public sealed partial class TypeFilterExtensionsTests
{
	public sealed class NameMatchTests
	{
		[Theory]
		[InlineData("TESTClass", false)]
		[InlineData("TestClass", true)]
		[InlineData("T???Class", true)]
		[InlineData("*Class", true)]
		[InlineData("Test*", true)]
		[InlineData("test*", false)]
		[InlineData("T*s", true)]
		[InlineData("*class", false)]
		public void WhichNameDoesNotMatch_CaseSensitive_ShouldReturnExpectedValue(
			string pattern, bool expectMatch)
		{
			ITypeFilter source = Expect.That.Types
				.WhichAre(typeof(TestClass)).And;

			ITypeFilterResult sut = source.WhichNameDoesNotMatch(pattern);

			ITestResult result = sut
				.ShouldAlwaysFail()
				.AllowEmpty()
				.Check.InAllLoadedAssemblies();
			sut.ToString().Should().Contain(
				$"name does not match pattern '{pattern}'");
			result.ShouldBeViolatedIf(!expectMatch);
		}

		[Theory]
		[InlineData("TESTClass", true)]
		[InlineData("TestClass", true)]
		[InlineData("t???class", true)]
		[InlineData("*class", true)]
		[InlineData("test*", true)]
		[InlineData("test???", false)]
		[InlineData("t*s", true)]
		public void WhichNameDoesNotMatch_WithIgnoreCase_ShouldReturnExpectedValue(
			string pattern, bool expectMatch)
		{
			ITypeFilter source = Expect.That.Types
				.WhichAre(typeof(TestClass)).And;

			ITypeFilterResult sut = source.WhichNameDoesNotMatch(pattern, true);

			ITestResult result = sut
				.ShouldAlwaysFail()
				.AllowEmpty()
				.Check.InAllLoadedAssemblies();
			sut.ToString().Should().Contain(
				$"name does not match pattern '{pattern}'");
			result.ShouldBeViolatedIf(!expectMatch);
		}

		[Theory]
		[InlineData("TESTClass", false)]
		[InlineData("TestClass", true)]
		[InlineData("T???Class", true)]
		[InlineData("*Class", true)]
		[InlineData("Test*", true)]
		[InlineData("test*", false)]
		[InlineData("T*s", true)]
		[InlineData("*class", false)]
		public void WhichNameMatches_CaseSensitive_ShouldReturnExpectedValue(
			string pattern, bool expectMatch)
		{
			ITypeFilter source = Expect.That.Types
				.WhichAre(typeof(TestClass)).And;

			ITypeFilterResult sut = source.WhichNameMatches(pattern);

			ITestResult result = sut
				.ShouldAlwaysFail()
				.AllowEmpty()
				.Check.InAllLoadedAssemblies();
			sut.ToString().Should().Contain(
				$"name matches pattern '{pattern}'");
			result.ShouldBeViolatedIf(expectMatch);
		}

		[Theory]
		[InlineData("TESTClass", true)]
		[InlineData("TestClass", true)]
		[InlineData("t???class", true)]
		[InlineData("*class", true)]
		[InlineData("test*", true)]
		[InlineData("test???", false)]
		[InlineData("t*s", true)]
		public void WhichNameMatches_WithIgnoreCase_ShouldReturnExpectedValue(
			string pattern, bool expectMatch)
		{
			ITypeFilter source = Expect.That.Types
				.WhichAre(typeof(TestClass)).And;

			ITypeFilterResult sut = source.WhichNameMatches(pattern, true);

			ITestResult result = sut
				.ShouldAlwaysFail()
				.AllowEmpty()
				.Check.InAllLoadedAssemblies();
			sut.ToString().Should().Contain(
				$"name matches pattern '{pattern}'");
			result.ShouldBeViolatedIf(expectMatch);
		}

		private class TestClass
		{
		}
	}
}
