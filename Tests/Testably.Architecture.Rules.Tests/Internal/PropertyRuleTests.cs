using AutoFixture.Xunit2;
using FluentAssertions;
using System.Linq;
using System.Reflection;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Internal;

public sealed class PropertyRuleTests
{
	[Fact]
	public void ShouldSatisfy_DefaultError_ShouldIncludePropertyInfoName()
	{
		PropertyInfo propertyInfo = typeof(DummyClass).GetProperties().First();
		string expectedPropertyInfoName = $"'{propertyInfo.Name}'";
		IRule rule = Expect.That.Properties
			.Which(t => t == propertyInfo)
			.ShouldSatisfy(_ => false);

		ITestResult result = rule.Check
			.In(typeof(DummyClass).Assembly);

		TestError error = result.Errors.Single();
		error.ToString().Should().Contain(expectedPropertyInfoName);
	}

	[Theory]
	[AutoData]
	public void ShouldSatisfy_False_ShouldIncludeError(TestError error)
	{
		PropertyInfo propertyInfo = typeof(DummyClass).GetProperties().First();
		IRule rule = Expect.That.Properties
			.Which(t => t == propertyInfo)
			.ShouldSatisfy(Requirement.ForProperty(_ => false, _ => error));

		ITestResult result = rule.Check
			.In(typeof(DummyClass).Assembly);

		result.Errors.Should().NotBeEmpty();
		result.Errors.Single().Should().Be(error);
	}

	[Theory]
	[AutoData]
	public void ShouldSatisfy_True_ShouldNotIncludeError(TestError error)
	{
		PropertyInfo propertyInfo = typeof(DummyClass).GetProperties().First();
		IRule rule = Expect.That.Properties
			.Which(t => t == propertyInfo)
			.ShouldSatisfy(Requirement.ForProperty(_ => true, _ => error));

		ITestResult result = rule.Check
			.In(typeof(DummyClass).Assembly);

		result.Errors.Should().BeEmpty();
	}

	[Fact]
	public void Which_ShouldFilterOutPropertyInfos()
	{
		int allPropertiesCount = typeof(DummyClass).GetProperties().Length;

		IRule rule = Expect.That.Properties
			.Which(t => t.DeclaringType == typeof(DummyClass)).And
			.Which(p => p.Name.StartsWith(nameof(DummyClass.DummyProperty1)))
			.ShouldSatisfy(Requirement.ForProperty(_ => false));

		ITestResult result = rule.Check
			.In(typeof(DummyClass).Assembly);

		result.Errors.Length.Should().BeLessThan(allPropertiesCount);
		result.Errors.Should().OnlyContain(e => e.ToString().Contains($"'{nameof(DummyClass.DummyProperty1)}"));
	}
}
