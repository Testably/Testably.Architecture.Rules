using AutoFixture.Xunit2;
using FluentAssertions;
using System.Linq;
using System.Reflection;
using Testably.Architecture.Rules.Internal;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Internal;

public sealed class FieldRuleTests
{
	[Theory]
	[InlineData(true, true, true)]
	[InlineData(true, false, false)]
	[InlineData(false, true, false)]
	[InlineData(false, false, false)]
	public void Applies_ShouldApplyAllFilters(bool result1, bool result2, bool expectedResult)
	{
		FieldInfo element = typeof(DummyFooClass).GetFields().First();

		FieldRule sut = new(
			Filter.FromPredicate<FieldInfo>(_ => result1),
			Filter.FromPredicate<FieldInfo>(_ => result2));

		bool result = sut.Applies(element);

		result.Should().Be(expectedResult);
	}

	[Fact]
	public void ShouldSatisfy_DefaultError_ShouldIncludeFieldInfoName()
	{
		FieldInfo fieldInfo = typeof(DummyFooClass).GetFields().First();
		string expectedFieldInfoName = $"'{fieldInfo.Name}'";
		IRule rule = Expect.That.Fields
			.Which(t => t == fieldInfo)
			.ShouldSatisfy(_ => false);

		ITestResult result = rule.Check
			.In(typeof(DummyFooClass).Assembly);

		TestError error = result.Errors.Single();
		error.ToString().Should().Contain(expectedFieldInfoName);
	}

	[Theory]
	[AutoData]
	public void ShouldSatisfy_False_ShouldIncludeError(TestError error)
	{
		FieldInfo fieldInfo = typeof(DummyFooClass).GetFields().First();
		IRule rule = Expect.That.Fields
			.Which(t => t == fieldInfo)
			.ShouldSatisfy(Requirement.ForField(_ => false, _ => error));

		ITestResult result = rule.Check
			.In(typeof(DummyFooClass).Assembly);

		result.Errors.Should().NotBeEmpty();
		result.Errors.Single().Should().Be(error);
	}

	[Theory]
	[AutoData]
	public void ShouldSatisfy_True_ShouldNotIncludeError(TestError error)
	{
		FieldInfo fieldInfo = typeof(DummyFooClass).GetFields().First();
		IRule rule = Expect.That.Fields
			.Which(t => t == fieldInfo)
			.ShouldSatisfy(Requirement.ForField(_ => true, _ => error));

		ITestResult result = rule.Check
			.In(typeof(DummyFooClass).Assembly);

		result.Errors.Should().BeEmpty();
	}

	[Theory]
	[AutoData]
	public void ToString_ShouldCombineFilters(string filter1, string filter2)
	{
		IRule rule = Expect.That.Fields
			.Which(_ => true, filter1).And
			.Which(_ => true, filter2)
			.ShouldSatisfy(_ => true);

		rule.ToString().Should().Be($"{filter1} and {filter2}");
	}

	[Theory]
	[AutoData]
	public void Types_ShouldApplyFieldFilter(string filterName)
	{
		FieldInfo origin = typeof(DummyFooClass).GetFields().First();

		IRule rule = Expect.That.Fields
			.Which(c => c == origin, filterName)
			.Types
			.Which(_ => false)
			.ShouldAlwaysFail();

		ITestResult result = rule.Check
			.In(typeof(DummyFooClass).Assembly);

		result.Errors.Length.Should().Be(1);
		result.Errors[0].ToString().Should()
			.Contain(filterName).And.Contain("type must have a field");
	}

	[Fact]
	public void Which_ShouldFilterOutFieldInfos()
	{
		int allFieldsCount = typeof(DummyFooClass).GetFields().Length;

		IRule rule = Expect.That.Fields
			.Which(t => t.DeclaringType == typeof(DummyFooClass)).And
			.Which(p => p.Name.StartsWith(nameof(DummyFooClass.DummyFooField1)))
			.ShouldSatisfy(Requirement.ForField(_ => false));

		ITestResult result = rule.Check
			.In(typeof(DummyFooClass).Assembly);

		result.Errors.Length.Should().BeLessThan(allFieldsCount);
		result.Errors.Should()
			.OnlyContain(e => e.ToString().Contains($"'{nameof(DummyFooClass.DummyFooField1)}"));
	}
}
