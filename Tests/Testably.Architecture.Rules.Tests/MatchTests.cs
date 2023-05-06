using FluentAssertions;
using Xunit;

namespace Testably.Architecture.Rules.Tests;

public sealed class MatchTests
{
	[Fact]
	public void Wildcard_MatchesNull_ShouldReturnFalse()
	{
		Match match = Match.Wildcard("*");

		bool matches = match.Matches(null, false);

		matches.Should().BeFalse();
	}

	[Theory]
	[InlineData("*", "Foo.Bar", true)]
	[InlineData("Foo", "Foo", true)]
	[InlineData("Foo", "xFoo", false)]
	[InlineData("Foo", "xFoo.Bar", false)]
	[InlineData("Foo?Bar", "Foo.Bar", true)]
	[InlineData("Foo?r", "Foo.Bar", false)]
	[InlineData("Foo*", "Foo.Bar", true)]
	[InlineData("Foo*r", "Foo.Bar", true)]
	public void Wildcard_ShouldReturnValidRegexMatchingExpectedResult(
		string wildcard, string testInput, bool expectedResult)
	{
		Match match = Match.Wildcard(wildcard);

		bool matches = match.Matches(testInput, false);

		matches.Should().Be(expectedResult,
			$"wildcard '{wildcard.Replace("$", "$$")}' should match '{testInput}'");
	}
}
