using AutoFixture.Xunit2;
using FluentAssertions;
using System.Linq;
using System.Reflection;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Internal;

public sealed class ConstructorRuleTests
{
	[Fact]
	public void ShouldSatisfy_DefaultError_ShouldIncludeConstructorInfoName()
	{
		ConstructorInfo constructorInfo = typeof(DummyClass).GetConstructors().First();
		string expectedConstructorInfoName = $"'{constructorInfo.Name}'";
		IRule rule = Expect.That.Constructors
			.Which(t => t == constructorInfo)
			.ShouldSatisfy(_ => false);

		ITestResult result = rule.Check
			.In(typeof(DummyClass).Assembly);

		TestError error = result.Errors.Single();
		error.ToString().Should().Contain(expectedConstructorInfoName);
	}

	[Theory]
	[AutoData]
	public void ShouldSatisfy_False_ShouldIncludeError(TestError error)
	{
		ConstructorInfo constructorInfo = typeof(DummyClass).GetConstructors().First();
		IRule rule = Expect.That.Constructors
			.Which(t => t == constructorInfo)
			.ShouldSatisfy(Requirement.ForConstructor(_ => false, _ => error));

		ITestResult result = rule.Check
			.In(typeof(DummyClass).Assembly);

		result.Errors.Should().NotBeEmpty();
		result.Errors.Single().Should().Be(error);
	}

	[Theory]
	[AutoData]
	public void ShouldSatisfy_True_ShouldNotIncludeError(TestError error)
	{
		ConstructorInfo constructorInfo = typeof(DummyClass).GetConstructors().First();
		IRule rule = Expect.That.Constructors
			.Which(t => t == constructorInfo)
			.ShouldSatisfy(Requirement.ForConstructor(_ => true, _ => error));

		ITestResult result = rule.Check
			.In(typeof(DummyClass).Assembly);

		result.Errors.Should().BeEmpty();
	}

	[Fact]
	public void Which_ShouldFilterOutConstructorInfos()
	{
		ConstructorInfo excludedConstructor = typeof(DummyClass).GetConstructors().First();
		int allConstructorsCount = typeof(DummyClass).GetConstructors().Length;

		IRule rule = Expect.That.Constructors
			.Which(t => t.DeclaringType == typeof(DummyClass)).And
			.Which(p => p == excludedConstructor)
			.ShouldSatisfy(Requirement.ForConstructor(_ => false));

		ITestResult result = rule.Check
			.In(typeof(DummyClass).Assembly);

		result.Errors.Length.Should().BeLessThan(allConstructorsCount);
		result.Errors.Should().OnlyContain(e => e.ToString().Contains("'.ctor'"));
	}
}
