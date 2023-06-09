﻿using AutoFixture.Xunit2;
using FluentAssertions;
using System.Linq;
using System.Reflection;
using Testably.Architecture.Rules.Internal;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Internal;

public sealed class PropertyRuleTests
{
	[Theory]
	[InlineData(true, true, true)]
	[InlineData(true, false, false)]
	[InlineData(false, true, false)]
	[InlineData(false, false, false)]
	public void Applies_ShouldApplyAllFilters(bool result1, bool result2, bool expectedResult)
	{
		PropertyInfo element = typeof(DummyFooClass).GetProperties().First();

		PropertyRule sut = new(
			Filter.FromPredicate<PropertyInfo>(_ => result1),
			Filter.FromPredicate<PropertyInfo>(_ => result2));

		bool result = sut.Applies(element);

		result.Should().Be(expectedResult);
	}

	[Fact]
	public void ShouldSatisfy_DefaultError_ShouldIncludePropertyInfoName()
	{
		PropertyInfo propertyInfo = typeof(DummyFooClass).GetProperties().First();
		string expectedPropertyInfoName = $"'{propertyInfo.Name}'";
		IRule rule = Expect.That.Properties
			.Which(t => t == propertyInfo)
			.ShouldSatisfy(_ => false);

		ITestResult result = rule.Check
			.In(typeof(DummyFooClass).Assembly);

		TestError error = result.Errors.Single();
		error.ToString().Should().Contain(expectedPropertyInfoName);
	}

	[Theory]
	[AutoData]
	public void ShouldSatisfy_False_ShouldIncludeError(TestError error)
	{
		PropertyInfo propertyInfo = typeof(DummyFooClass).GetProperties().First();
		IRule rule = Expect.That.Properties
			.Which(t => t == propertyInfo)
			.ShouldSatisfy(Requirement.ForProperty(_ => false, _ => error));

		ITestResult result = rule.Check
			.In(typeof(DummyFooClass).Assembly);

		result.Errors.Should().NotBeEmpty();
		result.Errors.Single().Should().Be(error);
	}

	[Theory]
	[AutoData]
	public void ShouldSatisfy_True_ShouldNotIncludeError(TestError error)
	{
		PropertyInfo propertyInfo = typeof(DummyFooClass).GetProperties().First();
		IRule rule = Expect.That.Properties
			.Which(t => t == propertyInfo)
			.ShouldSatisfy(Requirement.ForProperty(_ => true, _ => error));

		ITestResult result = rule.Check
			.In(typeof(DummyFooClass).Assembly);

		result.Errors.Should().BeEmpty();
	}

	[Theory]
	[AutoData]
	public void ToString_ShouldCombineFilters(string filter1, string filter2)
	{
		IRule rule = Expect.That.Properties
			.Which(_ => true, filter1).And
			.Which(_ => true, filter2)
			.ShouldSatisfy(_ => true);

		rule.ToString().Should().Be($"{filter1} and {filter2}");
	}

	[Theory]
	[AutoData]
	public void Types_ShouldApplyPropertyFilter(string filter1, string filter2)
	{
		PropertyInfo origin = typeof(DummyFooClass).GetProperties().First();

		IRule rule = Expect.That.Properties
			.Which(p => p == origin, filter1).And
			.Which(_ => true, filter2)
			.Types
			.Which(_ => false)
			.ShouldAlwaysFail();

		ITestResult result = rule.Check
			.In(typeof(DummyFooClass).Assembly);

		result.Errors.Length.Should().Be(1);
		result.Errors[0].ToString().Should()
			.Contain("type must have a property").And
			.Contain($"{filter1} and {filter2}");
	}

	[Fact]
	public void Types_ShouldRequireAllProperties()
	{
		PropertyInfo property1 = typeof(DummyFooClass).GetProperties().First();
		PropertyInfo property2 = typeof(DummyFooClass).GetProperties().Last();

		IRule rule = Expect.That.Properties
			.Which(p => p == property1).And
			.Which(p => p == property2)
			.Types
			.ShouldAlwaysFail()
			.AllowEmpty();

		ITestResult result = rule.Check
			.In(typeof(DummyFooClass).Assembly);

		result.ShouldNotBeViolated();
	}

	[Fact]
	public void Which_ShouldFilterOutPropertyInfos()
	{
		int allPropertiesCount = typeof(DummyFooClass).GetProperties().Length;

		IRule rule = Expect.That.Properties
			.Which(t => t.DeclaringType == typeof(DummyFooClass)).And
			.Which(p => p.Name.StartsWith(nameof(DummyFooClass.DummyFooProperty1)))
			.ShouldSatisfy(Requirement.ForProperty(_ => false));

		ITestResult result = rule.Check
			.In(typeof(DummyFooClass).Assembly);

		result.Errors.Length.Should().BeLessThan(allPropertiesCount);
		result.Errors.Should()
			.OnlyContain(e => e.ToString().Contains($"'{nameof(DummyFooClass.DummyFooProperty1)}"));
	}
}
