using AutoFixture.Xunit2;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Testably.Architecture.Rules.Internal;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;
using Xunit.Abstractions;

namespace Testably.Architecture.Rules.Tests.Internal;

public sealed class RuleCheckTests
{
	private readonly ITestOutputHelper _testOutputHelper;

	public RuleCheckTests(ITestOutputHelper testOutputHelper)
	{
		_testOutputHelper = testOutputHelper;
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
			.Which.ToString().Should()
			.Be($"No Int32 was found that matches the filter: {filterName}");
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
			.Which.ToString().Should()
			.Contain($"No Int32 was found that matches the {filterNames.Length} filters:" +
			         Environment.NewLine).And
			.Contain(" - " + string.Join(Environment.NewLine + " - ", filterNames));
	}

	[Theory]
	[AutoData]
	public void In_WithFilteredRequirements_ShouldBeViolated(TestError error)
	{
		RuleCheck<int> sut = new(
			new List<Filter<int>>(),
			new List<Requirement<int>>
			{
				Requirement.Create<int>(_ => false, _ => error)
			},
			new List<Exemption>(),
			TransformToInt);

		ITestResult result = sut.In(new FilteredTestDataProviderMock());

		result.ShouldBeViolated();
		result.Errors[0].Should().BeOfType<EmptySourceTestError>();
	}

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
	public void In_WithRequirements_ShouldBeViolated(TestError error)
	{
		RuleCheck<int> sut = new(
			new List<Filter<int>>(),
			new List<Requirement<int>>
			{
				Requirement.Create<int>(_ => false, _ => error)
			},
			new List<Exemption>(),
			TransformToInt);

		ITestResult result = sut.In(new TestDataProviderMock());

		result.ShouldBeViolated();
		result.Errors[0].Should().Be(error);
	}

	[Fact]
	public void WithLog_()
	{
		ITestResult result = Expect.That.Types
			.WhichArePublic().And
			.WhichHaveMethodWithAttribute<FactAttribute>().OrAttribute<TheoryAttribute>()
			.ShouldBeSealed()
			.Check.WithLog(_testOutputHelper.WriteLine).InAllLoadedAssemblies();

		result.ShouldNotBeViolated();
	}

	#region Helpers

	private static IEnumerable<int> TransformToInt(IEnumerable<Assembly> assemblies)
	{
		foreach (Assembly assembly in assemblies)
		{
			yield return assembly.GetHashCode();
		}
	}

	#endregion

	private class FilteredTestDataProviderMock : ITestDataProvider, IDataFilter<int>
	{
		#region IDataFilter<int> Members

		/// <inheritdoc />
		public IEnumerable<int> Filter(IEnumerable<int> source)
		{
			return Array.Empty<int>();
		}

		#endregion

		#region ITestDataProvider Members

		/// <inheritdoc cref="ITestDataProvider.GetAssemblies()" />
		public IEnumerable<Assembly> GetAssemblies()
		{
			yield return typeof(RuleCheckTests).Assembly;
		}

		#endregion
	}

	private class TestDataProviderMock : ITestDataProvider
	{
		#region ITestDataProvider Members

		/// <inheritdoc cref="ITestDataProvider.GetAssemblies()" />
		public IEnumerable<Assembly> GetAssemblies()
		{
			yield return typeof(RuleCheckTests).Assembly;
		}

		#endregion
	}
}
