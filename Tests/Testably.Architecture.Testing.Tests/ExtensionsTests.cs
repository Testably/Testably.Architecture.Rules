using FluentAssertions;
using System.Text.RegularExpressions;
using Xunit;

namespace Testably.Architecture.Testing.Tests;

public sealed class ExtensionsTests
{
	[Theory]
	[InlineData("Foo", "Foo", true)]
	[InlineData("Foo", "xFoo", false)]
	[InlineData("Foo", "xFoo.Bar", false)]
	[InlineData("Foo?Bar", "Foo.Bar", true)]
	[InlineData("Foo?r", "Foo.Bar", false)]
	[InlineData("Foo*r", "Foo.Bar", true)]
	[InlineData("Foo*", "Foo.Bar", true)]
	public void WildcardToRegular_ShouldReturnValidRegexMatchingExpectedResult(
		string wildcard, string testInput, bool expectedResult)
	{
		string regexPattern = Testing.Extensions.WildcardToRegular(wildcard);

		bool matches = Regex.IsMatch(testInput, regexPattern);

		matches.Should().Be(expectedResult,
			$"regex '{regexPattern.Replace("$", "$$")}' should match '{testInput}'");
	}
}
