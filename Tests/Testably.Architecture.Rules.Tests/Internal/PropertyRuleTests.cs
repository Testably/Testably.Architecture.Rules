using AutoFixture.Xunit2;
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
	public void Types_ShouldApplyPropertyFilter(string filterName)
	{
		PropertyInfo origin = typeof(DummyFooClass).GetProperties().First();

		IRule rule = Expect.That.Properties
			.Which(c => c == origin, filterName)
			.Types
			.Which(_ => false)
			.ShouldAlwaysFail();

		ITestResult result = rule.Check
			.In(typeof(DummyFooClass).Assembly);

		result.Errors.Length.Should().Be(1);
		result.Errors[0].ToString().Should()
			.Contain(filterName).And.Contain("type must have a property");
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
