using FluentAssertions;
using System;
using System.Linq;

namespace Testably.Architecture.Rules.Tests.TestHelpers;

internal static class TestResultExtensions
{
	public static ITestResult GetMatchingTypesAsErrorInAllLoadedAssemblies<TType>(
		this IRequirement<TType> @this)
	{
		return @this.ShouldAlwaysFail().Check.InAllLoadedAssemblies();
	}

	public static IRequirementResult<TType> ShouldAlwaysFail<TType>(this IRequirement<TType> @this)
	{
		return @this.ShouldSatisfy(
			Requirement.Create<TType>(
				_ => false,
				type => new TestError($"Expect {type} to fail.")));
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

	public static ITypeFilterResult WhichAre(this ITypeFilter typeFilter, params Type[] types)
	{
		return typeFilter.Which(t => types.Contains(t));
	}
}
