using AutoFixture.Xunit2;
using FluentAssertions;
using System.Linq;
using System.Reflection;
using Testably.Architecture.Rules.Internal;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Internal;

public sealed class ConstructorRuleTests
{
	[Theory]
	[InlineData(true, true, true)]
	[InlineData(true, false, false)]
	[InlineData(false, true, false)]
	[InlineData(false, false, false)]
	public void Applies_ShouldApplyAllFilters(bool result1, bool result2, bool expectedResult)
	{
		ConstructorInfo element = typeof(DummyFooClass).GetConstructors().First();

		ConstructorRule sut = new(
			Filter.FromPredicate<ConstructorInfo>(_ => result1),
			Filter.FromPredicate<ConstructorInfo>(_ => result2));

		bool result = sut.Applies(element);

		result.Should().Be(expectedResult);
	}

	[Fact]
	public void ShouldSatisfy_DefaultError_ShouldIncludeConstructorInfoName()
	{
		ConstructorInfo constructorInfo = typeof(DummyFooClass).GetConstructors().First();
		string expectedConstructorInfoName = $"'{constructorInfo.Name}'";
		IRule rule = Expect.That.Constructors
			.Which(t => t == constructorInfo)
			.ShouldSatisfy(_ => false);

		ITestResult result = rule.Check
			.In(typeof(DummyFooClass).Assembly);

		TestError error = result.Errors.Single();
		error.ToString().Should().Contain(expectedConstructorInfoName);
	}

	[Theory]
	[AutoData]
	public void ShouldSatisfy_False_ShouldIncludeError(TestError error)
	{
		ConstructorInfo constructorInfo = typeof(DummyFooClass).GetConstructors().First();
		IRule rule = Expect.That.Constructors
			.Which(t => t == constructorInfo)
			.ShouldSatisfy(Requirement.ForConstructor(_ => false, _ => error));

		ITestResult result = rule.Check
			.In(typeof(DummyFooClass).Assembly);

		result.Errors.Should().NotBeEmpty();
		result.Errors.Single().Should().Be(error);
	}

	[Theory]
	[AutoData]
	public void ShouldSatisfy_True_ShouldNotIncludeError(TestError error)
	{
		ConstructorInfo constructorInfo = typeof(DummyFooClass).GetConstructors().First();
		IRule rule = Expect.That.Constructors
			.Which(t => t == constructorInfo)
			.ShouldSatisfy(Requirement.ForConstructor(_ => true, _ => error));

		ITestResult result = rule.Check
			.In(typeof(DummyFooClass).Assembly);

		result.Errors.Should().BeEmpty();
	}

	[Theory]
	[AutoData]
	public void ToString_ShouldCombineFilters(string filter1, string filter2)
	{
		IRule rule = Expect.That.Constructors
			.Which(_ => true, filter1).And
			.Which(_ => true, filter2)
			.ShouldSatisfy(_ => true);

		rule.ToString().Should().Be($"{filter1} and {filter2}");
	}

	[Theory]
	[AutoData]
	public void Types_ShouldApplyConstructorFilter(string filter1, string filter2)
	{
		ConstructorInfo origin = typeof(DummyFooClass).GetConstructors().First();

		IRule rule = Expect.That.Constructors
			.Which(c => c == origin, filter1).And
			.Which(_ => true, filter2)
			.Types
			.Which(_ => false)
			.ShouldAlwaysFail();

		ITestResult result = rule.Check
			.In(typeof(DummyFooClass).Assembly);

		result.Errors.Length.Should().Be(1);
		result.Errors[0].ToString().Should()
			.Contain("type must have a constructor").And
			.Contain($"{filter1} and {filter2}");
	}

	[Fact]
	public void Types_ShouldRequireAllConstructors()
	{
		ConstructorInfo constructor1 = typeof(DummyFooClass).GetConstructors().First();
		ConstructorInfo constructor2 = typeof(DummyFooClass).GetConstructors().Last();

		IRule rule = Expect.That.Constructors
			.Which(p => p == constructor1).And
			.Which(p => p == constructor2)
			.Types
			.ShouldAlwaysFail()
			.AllowEmpty();

		ITestResult result = rule.Check
			.In(typeof(DummyFooClass).Assembly);

		result.ShouldNotBeViolated();
	}

	[Fact]
	public void Which_ShouldFilterOutConstructorInfos()
	{
		ConstructorInfo excludedConstructor = typeof(DummyFooClass).GetConstructors().First();
		int allConstructorsCount = typeof(DummyFooClass).GetConstructors().Length;

		IRule rule = Expect.That.Constructors
			.Which(t => t.DeclaringType == typeof(DummyFooClass)).And
			.Which(p => p == excludedConstructor)
			.ShouldSatisfy(Requirement.ForConstructor(_ => false));

		ITestResult result = rule.Check
			.In(typeof(DummyFooClass).Assembly);

		result.Errors.Length.Should().BeLessThan(allConstructorsCount);
		result.Errors.Should().OnlyContain(e => e.ToString().Contains("'.ctor'"));
	}
}
