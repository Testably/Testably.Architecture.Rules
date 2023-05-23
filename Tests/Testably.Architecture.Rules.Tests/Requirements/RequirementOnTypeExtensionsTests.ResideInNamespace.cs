using FluentAssertions;
using System;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Requirements;

public sealed partial class RequirementOnTypeExtensionsTests
{
	public sealed class ResideInNamespaceTests
	{
		[Theory]
		[InlineData(false)]
		[InlineData(true)]
		public void ShouldNotResideInNamespace_IgnoreCase_ShouldConsiderParameter(bool ignoreCase)
		{
			Type type = typeof(ResideInNamespaceTests);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldNotResideInNamespace("TESTABLY.architecture.RULES.tests.REQUIREMENTS",
					ignoreCase);

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolatedIf(ignoreCase);
		}

		[Theory]
		[InlineData("*")]
		[InlineData("Test????.Architecture.Rules.Tests.Requirements")]
		[InlineData("Testably.Architecture.Rules.Tests.Requirements")]
		public void ShouldNotResideInNamespace_MatchingPattern_ShouldNotBeSatisfied(
			string matchingPattern)
		{
			Type type = typeof(ResideInNamespaceTests);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldNotResideInNamespace(matchingPattern);

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
		[InlineData("Test??.Architecture.Rules.Tests.Requirements")]
		[InlineData("TESTABLY.architecture.RULES.tests.REQUIREMENTS")]
		public void ShouldNotResideInNamespace_NotMatchingPattern_ShouldNotBeViolated(
			string notMatchingPattern)
		{
			Type type = typeof(ResideInNamespaceTests);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldNotResideInNamespace(notMatchingPattern);

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
		}

		[Theory]
		[InlineData(false)]
		[InlineData(true)]
		public void ShouldResideInNamespace_IgnoreCase_ShouldConsiderParameter(bool ignoreCase)
		{
			Type type = typeof(ResideInNamespaceTests);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldResideInNamespace("TESTABLY.architecture.RULES.tests.REQUIREMENTS",
					ignoreCase);

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolatedIf(!ignoreCase);
		}

		[Theory]
		[InlineData("*")]
		[InlineData("Test????.Architecture.Rules.Tests.Requirements")]
		[InlineData("Testably.Architecture.Rules.Tests.Requirements")]
		public void ShouldResideInNamespace_MatchingPattern_ShouldNotBeViolated(
			string matchingPattern)
		{
			Type type = typeof(ResideInNamespaceTests);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldResideInNamespace(matchingPattern);

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
		}

		[Theory]
		[InlineData("*Foo")]
		[InlineData("Test??.Architecture.Rules.Tests.Requirements")]
		[InlineData("TESTABLY.architecture.RULES.tests.REQUIREMENTS")]
		public void ShouldResideInNamespace_NotMatchingPattern_ShouldNotBeSatisfied(
			string notMatchingPattern)
		{
			Type type = typeof(ResideInNamespaceTests);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldResideInNamespace(notMatchingPattern);

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolated();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.ToString().Should().Contain($"'{notMatchingPattern}'");
		}
	}
}
