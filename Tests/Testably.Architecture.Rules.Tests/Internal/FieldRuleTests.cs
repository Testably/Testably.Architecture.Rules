using AutoFixture.Xunit2;
using FluentAssertions;
using System.Linq;
using System.Reflection;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Internal;

public sealed class FieldRuleTests
{
	[Fact]
	public void ShouldSatisfy_DefaultError_ShouldIncludeFieldInfoName()
	{
		FieldInfo fieldInfo = typeof(DummyClass).GetFields().First();
		string expectedFieldInfoName = $"'{fieldInfo.Name}'";
		IRule rule = Expect.That.Fields
			.Which(t => t == fieldInfo)
			.ShouldSatisfy(_ => false);

		ITestResult result = rule.Check
			.In(typeof(DummyClass).Assembly);

		TestError error = result.Errors.Single();
		error.ToString().Should().Contain(expectedFieldInfoName);
	}

	[Theory]
	[AutoData]
	public void ShouldSatisfy_False_ShouldIncludeError(TestError error)
	{
		FieldInfo fieldInfo = typeof(DummyClass).GetFields().First();
		IRule rule = Expect.That.Fields
			.Which(t => t == fieldInfo)
			.ShouldSatisfy(Requirement.ForField(_ => false, _ => error));

		ITestResult result = rule.Check
			.In(typeof(DummyClass).Assembly);

		result.Errors.Should().NotBeEmpty();
		result.Errors.Single().Should().Be(error);
	}

	[Theory]
	[AutoData]
	public void ShouldSatisfy_True_ShouldNotIncludeError(TestError error)
	{
		FieldInfo fieldInfo = typeof(DummyClass).GetFields().First();
		IRule rule = Expect.That.Fields
			.Which(t => t == fieldInfo)
			.ShouldSatisfy(Requirement.ForField(_ => true, _ => error));

		ITestResult result = rule.Check
			.In(typeof(DummyClass).Assembly);

		result.Errors.Should().BeEmpty();
	}

	[Fact]
	public void Which_ShouldFilterOutFieldInfos()
	{
		int allFieldsCount = typeof(DummyClass).GetFields().Length;

		IRule rule = Expect.That.Fields
			.Which(t => t.DeclaringType == typeof(DummyClass)).And
			.Which(p => p.Name.StartsWith(nameof(DummyClass.DummyField1)))
			.ShouldSatisfy(Requirement.ForField(_ => false));

		ITestResult result = rule.Check
			.In(typeof(DummyClass).Assembly);

		result.Errors.Length.Should().BeLessThan(allFieldsCount);
		result.Errors.Should()
			.OnlyContain(e => e.ToString().Contains($"'{nameof(DummyClass.DummyField1)}"));
	}
}
