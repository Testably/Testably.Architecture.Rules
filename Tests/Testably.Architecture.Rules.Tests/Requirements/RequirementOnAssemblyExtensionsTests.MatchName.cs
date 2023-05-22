using FluentAssertions;
using System.Reflection;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Requirements;

public sealed partial class RequirementOnAssemblyExtensionsTests
{
	public sealed class MatchNameTests
	{
		[Theory]
		[InlineData(false)]
		[InlineData(true)]
		public void ShouldMatchName_IgnoreCase_ShouldConsiderParameter(bool ignoreCase)
		{
			IRule rule = Expect.That.Assemblies
				.ShouldMatchName("TESTABLY.architecture.RULES", ignoreCase);

			ITestResult result = rule.Check
				.InAssemblyContaining<ArchitectureRuleViolatedException>();

			result.ShouldBeViolatedIf(!ignoreCase);
		}

		[Theory]
		[InlineData("*")]
		[InlineData("????????.Architecture.Rules")]
		[InlineData("Testably.Architecture.Rules")]
		public void ShouldMatchName_MatchingPattern_ShouldNotBeViolated(string matchingPattern)
		{
			IRule rule = Expect.That.Assemblies
				.ShouldMatchName(matchingPattern);

			ITestResult result = rule.Check
				.InAssemblyContaining<ArchitectureRuleViolatedException>();

			result.ShouldNotBeViolated();
		}

		[Theory]
		[InlineData("*Foo")]
		[InlineData("??.Architecture.Rules")]
		[InlineData("testably.architecture.rules")]
		public void ShouldMatchName_NotMatchingPattern_ShouldNotBeSatisfied(
			string notMatchingPattern)
		{
			Assembly expectedAssembly = typeof(ArchitectureRuleViolatedException).Assembly;
			IRule rule = Expect.That.Assemblies
				.ShouldMatchName(notMatchingPattern);

			ITestResult result = rule.Check
				.In(expectedAssembly);

			result.ShouldBeViolated();
			result.Errors[0].Should().BeOfType<AssemblyTestError>()
				.Which.Assembly.Should().BeSameAs(expectedAssembly);
			result.Errors[0].Should().BeOfType<AssemblyTestError>()
				.Which.ToString().Should().Contain($"'{notMatchingPattern}'");
		}

		[Theory]
		[InlineData(false)]
		[InlineData(true)]
		public void ShouldNotMatchName_IgnoreCase_ShouldConsiderParameter(bool ignoreCase)
		{
			IRule rule = Expect.That.Assemblies
				.ShouldNotMatchName("TESTABLY.architecture.RULES", ignoreCase);

			ITestResult result = rule.Check
				.InAssemblyContaining<ArchitectureRuleViolatedException>();

			result.ShouldBeViolatedIf(ignoreCase);
		}

		[Theory]
		[InlineData("*")]
		[InlineData("????????.Architecture.Rules")]
		[InlineData("Testably.Architecture.Rules")]
		public void ShouldNotMatchName_MatchingPattern_ShouldNotBeSatisfied(string matchingPattern)
		{
			Assembly expectedAssembly = typeof(ArchitectureRuleViolatedException).Assembly;
			IRule rule = Expect.That.Assemblies
				.ShouldNotMatchName(matchingPattern);

			ITestResult result = rule.Check
				.In(expectedAssembly);

			result.ShouldBeViolated();
			result.Errors[0].Should().BeOfType<AssemblyTestError>()
				.Which.Assembly.Should().BeSameAs(expectedAssembly);
			result.Errors[0].Should().BeOfType<AssemblyTestError>()
				.Which.ToString().Should().Contain($"'{matchingPattern}'");
		}

		[Theory]
		[InlineData("*Foo")]
		[InlineData("??.Architecture.Rules")]
		[InlineData("testably.architecture.rules")]
		public void ShouldNotMatchName_NotMatchingPattern_ShouldNotBeViolated(
			string notMatchingPattern)
		{
			IRule rule = Expect.That.Assemblies
				.ShouldNotMatchName(notMatchingPattern);

			ITestResult result = rule.Check
				.InAssemblyContaining<ArchitectureRuleViolatedException>();

			result.ShouldNotBeViolated();
		}
	}
}
