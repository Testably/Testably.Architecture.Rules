using FluentAssertions;
using System;
using System.Linq;
using System.Reflection;

namespace Testably.Architecture.Rules.Tests.TestHelpers;

internal static class TestResultExtensions
{
	public static IRequirementResult<Assembly> ShouldAlwaysFail(this IRequirement<Assembly> @this)
	{
		return @this.ShouldSatisfy(
			Requirement.Create<Assembly>(
				_ => false,
				type => new TestError($"Expect {type} to fail.")));
	}

	public static IRequirementResult<Type> ShouldAlwaysFail(this IRequirement<Type> @this)
	{
		return @this.ShouldSatisfy(
			Requirement.Create<Type>(
				_ => false,
				type => new TestError($"Expect {type.FullName} to fail.")));
	}

	public static ITestResult ShouldBeViolated(this ITestResult result)
	{
		result.IsViolated.Should().BeTrue();
		result.Errors.Should().NotBeEmpty();
		return result;
	}

	public static ITestResult ShouldBeViolatedIf(this ITestResult result, bool expectIsViolated)
	{
		if (expectIsViolated)
		{
			return ShouldBeViolated(result);
		}

		return ShouldNotBeViolated(result);
	}

	public static ITestResult ShouldNotBeViolated(this ITestResult result)
	{
		result.IsViolated.Should().BeFalse(because: result.ToString());
		result.Errors.Should().BeEmpty();
		return result;
	}
}
