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
		MethodInfo element = typeof(DummyClass).GetMethods().First();

		MethodRule sut = new(
			Filter.FromPredicate<MethodInfo>(_ => result1),
			Filter.FromPredicate<MethodInfo>(_ => result2));

		bool result = sut.Applies(element);

		result.Should().Be(expectedResult);
	}

	[Fact]
	public void ShouldSatisfy_DefaultError_ShouldIncludeMethodInfoName()
	{
		MethodInfo methodInfo = typeof(DummyClass).GetDeclaredMethods().First();
		string expectedMethodInfoName = $"'{methodInfo.Name}'";
		IRule rule = Expect.That.Methods
			.Which(t => t == methodInfo)
			.ShouldSatisfy(_ => false);

		ITestResult result = rule.Check
			.In(typeof(DummyClass).Assembly);

		TestError error = result.Errors.Single();
		error.ToString().Should().Contain(expectedMethodInfoName);
	}

	[Theory]
	[AutoData]
	public void ShouldSatisfy_False_ShouldIncludeError(TestError error)
	{
		MethodInfo methodInfo = typeof(DummyClass).GetDeclaredMethods().First();
		IRule rule = Expect.That.Methods
			.Which(t => t == methodInfo)
			.ShouldSatisfy(Requirement.ForMethod(_ => false, _ => error));

		ITestResult result = rule.Check
			.In(typeof(DummyClass).Assembly);

		result.Errors.Should().NotBeEmpty();
		result.Errors.Single().Should().Be(error);
	}

	[Theory]
	[AutoData]
	public void ShouldSatisfy_True_ShouldNotIncludeError(TestError error)
	{
		MethodInfo methodInfo = typeof(DummyClass).GetDeclaredMethods().First();
		IRule rule = Expect.That.Methods
			.Which(t => t == methodInfo)
			.ShouldSatisfy(Requirement.ForMethod(_ => true, _ => error));

		ITestResult result = rule.Check
			.In(typeof(DummyClass).Assembly);

		result.Errors.Should().BeEmpty();
	}

	[Theory]
	[AutoData]
	public void Types_ShouldApplyMethodFilter(string filterName)
	{
		MethodInfo origin = typeof(DummyClass).GetMethods().First();

		IRule rule = Expect.That.Methods
			.Which(c => c == origin, filterName)
			.Types
			.Which(_ => false)
			.ShouldAlwaysFail();

		ITestResult result = rule.Check
			.In(typeof(DummyClass).Assembly);

		result.Errors.Length.Should().Be(1);
		result.Errors[0].ToString().Should()
			.Contain(filterName).And.Contain("type must have a method");
	}

	[Fact]
	public void Which_ShouldFilterOutMethodInfos()
	{
		int allMethodsCount = typeof(DummyClass).GetDeclaredMethods().Length;

		IRule rule = Expect.That.Methods
			.Which(t => t.DeclaringType == typeof(DummyClass)).And
			.Which(p => p.Name.StartsWith(nameof(DummyClass.DummyMethod1)))
			.ShouldSatisfy(Requirement.ForMethod(_ => false));

		ITestResult result = rule.Check
			.In(typeof(DummyClass).Assembly);

		result.Errors.Length.Should().BeLessThan(allMethodsCount);
		result.Errors.Should()
			.OnlyContain(e => e.ToString().Contains($"'{nameof(DummyClass.DummyMethod1)}"));
	}
}
