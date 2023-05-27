using AutoFixture.Xunit2;
using FluentAssertions;
using System.Linq;
using System.Reflection;
using Testably.Architecture.Rules.Internal;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Internal;

public sealed class MethodRuleTests
{
	[Theory]
	[InlineData(true, true, true)]
	[InlineData(true, false, false)]
	[InlineData(false, true, false)]
	[InlineData(false, false, false)]
	public void Applies_ShouldApplyAllFilters(bool result1, bool result2, bool expectedResult)
	{
		MethodInfo element = typeof(DummyFooClass).GetMethods().First();

		MethodRule sut = new(
			Filter.FromPredicate<MethodInfo>(_ => result1),
			Filter.FromPredicate<MethodInfo>(_ => result2));

		bool result = sut.Applies(element);

		result.Should().Be(expectedResult);
	}

	[Fact]
	public void ShouldSatisfy_DefaultError_ShouldIncludeMethodInfoName()
	{
		MethodInfo methodInfo = typeof(DummyFooClass).GetDeclaredMethods().First();
		string expectedMethodInfoName = $"'{methodInfo.Name}'";
		IRule rule = Expect.That.Methods
			.Which(t => t == methodInfo)
			.ShouldSatisfy(_ => false);

		ITestResult result = rule.Check
			.In(typeof(DummyFooClass).Assembly);

		TestError error = result.Errors.Single();
		error.ToString().Should().Contain(expectedMethodInfoName);
	}

	[Theory]
	[AutoData]
	public void ShouldSatisfy_False_ShouldIncludeError(TestError error)
	{
		MethodInfo methodInfo = typeof(DummyFooClass).GetDeclaredMethods().First();
		IRule rule = Expect.That.Methods
			.Which(t => t == methodInfo)
			.ShouldSatisfy(Requirement.ForMethod(_ => false, _ => error));

		ITestResult result = rule.Check
			.In(typeof(DummyFooClass).Assembly);

		result.Errors.Should().NotBeEmpty();
		result.Errors.Single().Should().Be(error);
	}

	[Theory]
	[AutoData]
	public void ShouldSatisfy_True_ShouldNotIncludeError(TestError error)
	{
		MethodInfo methodInfo = typeof(DummyFooClass).GetDeclaredMethods().First();
		IRule rule = Expect.That.Methods
			.Which(t => t == methodInfo)
			.ShouldSatisfy(Requirement.ForMethod(_ => true, _ => error));

		ITestResult result = rule.Check
			.In(typeof(DummyFooClass).Assembly);

		result.Errors.Should().BeEmpty();
	}

	[Theory]
	[AutoData]
	public void ToString_ShouldCombineFilters(string filter1, string filter2)
	{
		IRule rule = Expect.That.Methods
			.Which(_ => true, filter1).And
			.Which(_ => true, filter2)
			.ShouldSatisfy(_ => true);

		rule.ToString().Should().Be($"{filter1} and {filter2}");
	}

	[Theory]
	[AutoData]
	public void Types_ShouldApplyMethodFilter(string filterName)
	{
		MethodInfo origin = typeof(DummyFooClass).GetMethods().First();

		IRule rule = Expect.That.Methods
			.Which(c => c == origin, filterName)
			.Types
			.Which(_ => false)
			.ShouldAlwaysFail();

		ITestResult result = rule.Check
			.In(typeof(DummyFooClass).Assembly);

		result.Errors.Length.Should().Be(1);
		result.Errors[0].ToString().Should()
			.Contain(filterName).And.Contain("type must have a method");
	}

	[Fact]
	public void Types_ShouldRequireAllMethods()
	{
		MethodInfo method1 = typeof(DummyFooClass).GetMethods().First();
		MethodInfo method2 = typeof(DummyFooClass).GetMethods().Last();

		IRule rule = Expect.That.Methods
			.Which(p => p == method1).And
			.Which(p => p == method2)
			.Types
			.ShouldAlwaysFail()
			.AllowEmpty();

		ITestResult result = rule.Check
			.In(typeof(DummyFooClass).Assembly);

		result.ShouldNotBeViolated();
	}

	[Fact]
	public void Which_ShouldFilterOutMethodInfos()
	{
		int allMethodsCount = typeof(DummyFooClass).GetDeclaredMethods().Length;

		IRule rule = Expect.That.Methods
			.Which(t => t.DeclaringType == typeof(DummyFooClass)).And
			.Which(p => p.Name.StartsWith(nameof(DummyFooClass.DummyFooMethod1)))
			.ShouldSatisfy(Requirement.ForMethod(_ => false));

		ITestResult result = rule.Check
			.In(typeof(DummyFooClass).Assembly);

		result.Errors.Length.Should().BeLessThan(allMethodsCount);
		result.Errors.Should()
			.OnlyContain(e => e.ToString().Contains($"'{nameof(DummyFooClass.DummyFooMethod1)}"));
	}
}
