using AutoFixture.Xunit2;
using FluentAssertions;
using System;
using System.Linq.Expressions;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Requirements;

public sealed partial class RequirementOnTypeExtensionsTests
{
	[Fact]
	public void ShouldSatisfy_Expression_ShouldContainExpressionString()
	{
		Type type = typeof(RequirementOnTypeExtensionsTests);
		Expression<Func<Type, bool>> expression = _ => false;
		IRule rule = Expect.That.Types
			.WhichAre(type)
			.ShouldSatisfy(expression);

		ITestResult result = rule.Check
			.InAllLoadedAssemblies();

		result.ShouldBeViolated();
		result.Errors[0].Should().BeOfType<TypeTestError>()
			.Which.ToString().Should().Contain(expression.ToString());
	}

	[Theory]
	[AutoData]
	public void Should_ConstructorFilter_ShouldContainFilterName(string filterName)
	{
		Type type = typeof(DummyClass);
		IRule rule = Expect.That.Types
			.WhichAre(type)
			.Should(Have.Constructor.Which(_ => false, filterName));

		ITestResult result = rule.Check
			.InAllLoadedAssemblies();

		result.ShouldBeViolated();
		result.Errors[0].Should().BeOfType<ConstructorTestError>()
			.Which.ToString().Should()
			.Contain("type should have a constructor").And.Contain(filterName);
	}

	[Theory]
	[AutoData]
	public void Should_EventFilter_ShouldContainFilterName(string filterName)
	{
		Type type = typeof(DummyClass);
		IRule rule = Expect.That.Types
			.WhichAre(type)
			.Should(Have.Event.Which(_ => false, filterName));

		ITestResult result = rule.Check
			.InAllLoadedAssemblies();

		result.ShouldBeViolated();
		result.Errors[0].Should().BeOfType<EventTestError>()
			.Which.ToString().Should()
			.Contain("type should have an event").And.Contain(filterName);
	}

	[Theory]
	[AutoData]
	public void Should_FieldFilter_ShouldContainFilterName(string filterName)
	{
		Type type = typeof(DummyClass);
		IRule rule = Expect.That.Types
			.WhichAre(type)
			.Should(Have.Field.Which(_ => false, filterName));

		ITestResult result = rule.Check
			.InAllLoadedAssemblies();

		result.ShouldBeViolated();
		result.Errors[0].Should().BeOfType<FieldTestError>()
			.Which.ToString().Should()
			.Contain("type should have a field").And.Contain(filterName);
	}

	[Theory]
	[AutoData]
	public void Should_MethodFilter_ShouldContainFilterName(string filterName)
	{
		Type type = typeof(DummyClass);
		IRule rule = Expect.That.Types
			.WhichAre(type)
			.Should(Have.Method.Which(_ => false, filterName));

		ITestResult result = rule.Check
			.InAllLoadedAssemblies();

		result.ShouldBeViolated();
		result.Errors[0].Should().BeOfType<MethodTestError>()
			.Which.ToString().Should()
			.Contain("type should have a method").And.Contain(filterName);
	}

	[Theory]
	[AutoData]
	public void Should_PropertyFilter_ShouldContainFilterName(string filterName)
	{
		Type type = typeof(DummyClass);
		IRule rule = Expect.That.Types
			.WhichAre(type)
			.Should(Have.Property.Which(_ => false, filterName));

		ITestResult result = rule.Check
			.InAllLoadedAssemblies();

		result.ShouldBeViolated();
		result.Errors[0].Should().BeOfType<PropertyTestError>()
			.Which.ToString().Should()
			.Contain("type should have a property").And.Contain(filterName);
	}
}
