using FluentAssertions;
using System;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Requirements;

public sealed partial class RequirementOnTypeExtensionsTests
{
	public sealed class MatchNameTests
	{
		[Theory]
		[InlineData(false)]
		[InlineData(true)]
		public void ShouldMatchName_IgnoreCase_ShouldConsiderParameter(bool ignoreCase)
		{
			Type type = typeof(MatchNameTests);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldMatchName("MATCHnameTESTS", ignoreCase);

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolatedIf(!ignoreCase);
		}

		[Theory]
		[InlineData("*")]
		[InlineData("?????NameTests")]
		[InlineData("MatchNameTests")]
		public void ShouldMatchName_MatchingPattern_ShouldNotBeViolated(string matchingPattern)
		{
			Type type = typeof(MatchNameTests);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldMatchName(matchingPattern);

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
		}

		[Theory]
		[InlineData("*Foo")]
		[InlineData("??NameTests")]
		[InlineData("matchnametests")]
		public void ShouldMatchName_NotMatchingPattern_ShouldNotBeSatisfied(
			string notMatchingPattern)
		{
			Type type = typeof(MatchNameTests);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldMatchName(notMatchingPattern);

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolated();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.ToString().Should().Contain($"'{notMatchingPattern}'");
		}

		[Theory]
		[InlineData(false)]
		[InlineData(true)]
		public void ShouldNotMatchName_IgnoreCase_ShouldConsiderParameter(bool ignoreCase)
		{
			Type type = typeof(MatchNameTests);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldNotMatchName("matchNAMEtests", ignoreCase);

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolatedIf(ignoreCase);
		}

		[Theory]
		[InlineData("*")]
		[InlineData("?????NameTests")]
		[InlineData("MatchNameTests")]
		public void ShouldNotMatchName_MatchingPattern_ShouldNotBeSatisfied(string matchingPattern)
		{
			Type type = typeof(MatchNameTests);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldNotMatchName(matchingPattern);

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolated();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.ToString().Should().Contain($"'{matchingPattern}'");
		}

		[Theory]
		[InlineData("*Foo")]
		[InlineData("??NameTests")]
		[InlineData("matchnametests")]
		public void ShouldNotMatchName_NotMatchingPattern_ShouldNotBeViolated(
			string notMatchingPattern)
		{
			Type type = typeof(MatchNameTests);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldNotMatchName(notMatchingPattern);

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
		}
	}
}
