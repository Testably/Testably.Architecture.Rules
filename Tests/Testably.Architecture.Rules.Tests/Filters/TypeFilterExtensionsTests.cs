using AutoFixture.Xunit2;
using FluentAssertions;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Filters;

public sealed partial class TypeFilterExtensionsTests
{
	[Fact]
	public void And_WithContraryConditions_ShouldReturnEmptyArray()
	{
		IRule rule = Expect.That.Types
			.WhichArePublic().And.WhichAreNotPublic()
			.ShouldAlwaysFail()
			.AllowEmpty();

		ITestResult result = rule.Check
			.InExecutingAssembly();

		result.ShouldNotBeViolated();
	}

	[Theory]
	[InlineAutoData(false)]
	[InlineAutoData(true)]
	public void Which_WithConstructorFilter_ShouldIncludeFilterNameInToString(
		bool predicateResult, string filterName)
	{
		ITypeFilterResult sut = Expect.That.Types
			.Which(Have.Constructor.Which(_ => predicateResult, filterName));

		ITestResult result = sut
			.ShouldAlwaysFail()
			.AllowEmpty()
			.Check.InAllLoadedAssemblies();

		sut.ToString().Should()
			.Contain("has constructor").And.Contain(filterName);
		result.ShouldBeViolatedIf(predicateResult);
	}

	[Theory]
	[InlineAutoData(false)]
	[InlineAutoData(true)]
	public void Which_WithEventFilter_ShouldIncludeFilterNameInToString(
		bool predicateResult, string filterName)
	{
		ITypeFilterResult sut = Expect.That.Types
			.Which(Have.Event.Which(_ => predicateResult, filterName));

		ITestResult result = sut
			.ShouldAlwaysFail()
			.AllowEmpty()
			.Check.InAllLoadedAssemblies();

		sut.ToString().Should()
			.Contain("has event").And.Contain(filterName);
		result.ShouldBeViolatedIf(predicateResult);
	}

	[Theory]
	[InlineData(false)]
	[InlineData(true)]
	public void Which_WithExpression_ShouldConsiderPredicateResult(bool predicateResult)
	{
		ITypeFilterResult sut = Expect.That.Types
			.Which(_ => predicateResult);

		ITestResult result = sut
			.ShouldAlwaysFail()
			.AllowEmpty()
			.Check.InAllLoadedAssemblies();

		result.ShouldBeViolatedIf(predicateResult);
	}

	[Theory]
	[InlineAutoData(false)]
	[InlineAutoData(true)]
	public void Which_WithFieldFilter_ShouldIncludeFilterNameInToString(
		bool predicateResult, string filterName)
	{
		ITypeFilterResult sut = Expect.That.Types
			.Which(Have.Field.Which(_ => predicateResult, filterName));

		ITestResult result = sut
			.ShouldAlwaysFail()
			.AllowEmpty()
			.Check.InAllLoadedAssemblies();

		sut.ToString().Should()
			.Contain("has field").And.Contain(filterName);
		result.ShouldBeViolatedIf(predicateResult);
	}

	[Theory]
	[InlineAutoData(false)]
	[InlineAutoData(true)]
	public void Which_WithMethodFilter_ShouldIncludeFilterNameInToString(
		bool predicateResult, string filterName)
	{
		ITypeFilterResult sut = Expect.That.Types
			.Which(Have.Method.Which(_ => predicateResult, filterName));

		ITestResult result = sut
			.ShouldAlwaysFail()
			.AllowEmpty()
			.Check.InAllLoadedAssemblies();

		sut.ToString().Should()
			.Contain("has method").And.Contain(filterName);
		result.ShouldBeViolatedIf(predicateResult);
	}

	[Theory]
	[InlineAutoData(false)]
	[InlineAutoData(true)]
	public void Which_WithName_ShouldConsiderPredicateResult(bool predicateResult, string name)
	{
		ITypeFilterResult sut = Expect.That.Types
			.Which(_ => predicateResult, name);

		ITestResult result = sut
			.ShouldAlwaysFail()
			.AllowEmpty()
			.Check.InAllLoadedAssemblies();

		result.ShouldBeViolatedIf(predicateResult);
	}

	[Theory]
	[InlineAutoData(false)]
	[InlineAutoData(true)]
	public void Which_WithPropertyFilter_ShouldIncludeFilterNameInToString(
		bool predicateResult, string filterName)
	{
		ITypeFilterResult sut = Expect.That.Types
			.Which(Have.Property.Which(_ => predicateResult, filterName));

		ITestResult result = sut
			.ShouldAlwaysFail()
			.AllowEmpty()
			.Check.InAllLoadedAssemblies();

		sut.ToString().Should()
			.Contain("has property").And.Contain(filterName);
		result.ShouldBeViolatedIf(predicateResult);
	}
}
