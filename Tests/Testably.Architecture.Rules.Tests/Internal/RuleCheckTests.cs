using AutoFixture.Xunit2;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Testably.Architecture.Rules.Internal;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Internal;

public sealed class RuleCheckTests
{
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
	public void WithLog_Null_ShouldNotThrowException()
	{
		Exception? exception = Record.Exception(() =>
		{
			Expect.That.Types
				.WhichAre(AccessModifiers.Public).And
				.Which(Have.Method.WithAttribute<FactAttribute>().OrAttribute<TheoryAttribute>())
				.ShouldBeSealed()
				.Check.WithLog(null).InAllLoadedAssemblies();
		});

		exception.Should().BeNull();
	}

	[Theory]
	[AutoData]
	public void WithLog_ShouldIncludeErrorCount(TestError[] errors)
	{
		List<string> logs = new();
		IRequirement<Type> typeRequirement = Expect.That.Types
			.Which(t => t == GetType());
		foreach (TestError error in errors)
		{
			typeRequirement = typeRequirement
				.ShouldSatisfy(Requirement.ForType(_ => false, _ => error)).And;
		}

		typeRequirement.ShouldSatisfy(_ => true)
			.Check.WithLog(logs.Add).InAllLoadedAssemblies();

		logs.Should().Contain(log => log.Contains($"{errors.Length} errors in Type {GetType()}"));
		logs.Should().Contain(log => log.Contains($"{errors.Length} total errors"));
		foreach (string error in errors)
		{
			logs.Should().Contain(log => log.Contains(error));
		}
	}

	[Theory]
	[AutoData]
	public void WithLog_ShouldIncludeExemptionCount(string[] exemptionNames)
	{
		List<string> logs = new();
		IExemption<Type> exemption = Expect.That.Types
			.Which(t => t == GetType())
			.ShouldSatisfy(_ => false);

		foreach (string exemptionName in exemptionNames)
		{
			exemption = exemption
				.Unless(Exemption.FromPredicate(_ => false, exemptionName)).And;
		}

		exemption.Unless(Exemption.FromPredicate(_ => false))
			.Check.WithLog(logs.Add).InAllLoadedAssemblies();

		logs.Should().Contain(log => log.Contains($"{exemptionNames.Length + 1} exemptions"));
		foreach (string exemptionName in exemptionNames)
		{
			logs.Should().Contain(log => log.Contains($"Apply exemption {exemptionName}"));
		}
	}

	[Theory]
	[AutoData]
	public void WithLog_ShouldIncludeFilterCount(string[] filterNames)
	{
		List<string> logs = new();
		ITypeFilter typeFilter = Expect.That.Types;
		foreach (string filterName in filterNames)
		{
			typeFilter = typeFilter.Which(Filter.FromPredicate<Type>(_ => true, filterName)).And;
		}

		typeFilter.Which(_ => false)
			.ShouldBeSealed()
			.Check.WithLog(logs.Add).InAllLoadedAssemblies();

		logs.Should().Contain(log
			=> log.Contains($"before applying {filterNames.Length + 1} filters"));
		logs.Should().Contain(log
			=> log.Contains($"after applying {filterNames.Length + 1} filters"));
		foreach (string filterName in filterNames)
		{
			logs.Should().Contain(log => log.Contains($"Apply filter {filterName}"));
		}
	}

	[Fact]
	public void WithLog_ShouldIncludeTime()
	{
		List<string> logs = new();
		DateTime begin = DateTime.Now;
		Expect.That.Types
			.WhichAre(AccessModifiers.Public).And
			.Which(Have.Method.WithAttribute<FactAttribute>().OrAttribute<TheoryAttribute>())
			.ShouldBeSealed()
			.Check.WithLog(logs.Add).InAllLoadedAssemblies();
		DateTime end = DateTime.Now;
		DateTime middleTime = begin.AddSeconds((end - begin).TotalSeconds / 2);

		logs.Should().Contain(log => log.StartsWith(middleTime.ToString("HH:mm:ss")));
	}

	[Fact]
	public void WithLog_ShouldIndentErrorsWithMultipleLines()
	{
		string errorMessage = $"foo{Environment.NewLine}bar";
		List<string> logs = new();
		Expect.That.Types
			.Which(t => t == GetType())
			.ShouldSatisfy(Requirement.ForType(_ => false, _ => new TestError(errorMessage)))
			.Check.WithLog(logs.Add).InAllLoadedAssemblies();

		logs.Should().Contain(log => log.Contains($"foo{Environment.NewLine}"));
		logs.Should().Contain(log => log.Contains("  bar"));
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
