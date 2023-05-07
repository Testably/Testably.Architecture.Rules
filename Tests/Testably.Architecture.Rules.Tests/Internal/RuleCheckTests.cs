using AutoFixture.Xunit2;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Testably.Architecture.Rules.Internal;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Internal;

public sealed class RuleCheckTests
{
	[Fact]
	public void In_WithoutRequirements_ShouldNotBeViolated()
	{
		RuleCheck<int> sut = new(
			new List<Filter<int>>(),
			new List<Requirement<int>>(),
			new List<Exemption>(),
			TransformToInt);

		ITestResult result = sut.In(new TestDataProviderMock());

		result.ShouldNotBeViolated();
	}

	[Theory]
	[AutoData]
	public void In_WhenAllSourceIsFilteredOut_ShouldBeViolatedWithSingleEmptySourceTestError(
		string filterName)
	{
		RuleCheck<int> sut = new(
			new List<Filter<int>>
			{
				Filter.FromPredicate<int>(_ => false, filterName)
			},
			new List<Requirement<int>>(),
			new List<Exemption>(),
			TransformToInt);

		ITestResult result = sut.In(new TestDataProviderMock());

		result.ShouldBeViolated();
		result.Errors.Length.Should().Be(1);
		result.Errors[0].Should().BeOfType<EmptySourceTestError>()
			.Which.ToString().Should().Contain(filterName);
	}

	[Theory]
	[AutoData]
	public void
		In_WhenAllSourceIsFilteredOutByMultipleFilters_ShouldBeViolatedWithSingleEmptySourceTestError(
			string[] filterNames)
	{
		RuleCheck<int> sut = new(
			filterNames
				.Select(filterName => Filter.FromPredicate<int>(_ => false, filterName))
				.ToList(),
			new List<Requirement<int>>(),
			new List<Exemption>(),
			TransformToInt);

		ITestResult result = sut.In(new TestDataProviderMock());

		result.ShouldBeViolated();
		result.Errors.Length.Should().Be(1);
		result.Errors[0].Should().BeOfType<EmptySourceTestError>()
			.Which.ToString().Should().Contain($"{filterNames.Length} filters");
		foreach (string filterName in filterNames)
		{
			result.Errors[0].ToString().Should().Contain(filterName);
		}
	}

	private static IEnumerable<int> TransformToInt(IEnumerable<Assembly> assemblies)
	{
		foreach (Assembly assembly in assemblies)
		{
			yield return assembly.GetHashCode();
		}
	}

	private class TestDataProviderMock : ITestDataProvider
	{
		/// <inheritdoc cref="ITestDataProvider.GetAssemblies()" />
		public IEnumerable<Assembly> GetAssemblies()
		{
			yield return typeof(RuleCheckTests).Assembly;
		}
	}
}
