using AutoFixture.Xunit2;
using FluentAssertions;
using System;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Internal;

public sealed class TestResultTests
{
	[Theory]
	[AutoData]
	public void And_FirstFailed_ShouldIncludeOnlyFirstError(
		TestError error1, TestError error2)
	{
		ITestResult result = Expect.That.Assemblies
			.ShouldSatisfy(Requirement.ForAssembly(_ => false, _ => error1)).And
			.ShouldSatisfy(Requirement.ForAssembly(_ => true, _ => error2))
			.Check.InExecutingAssembly();

		result.ShouldBeViolated();
		result.Errors[0].Should().Be(error1);
		result.Errors.Length.Should().Be(1);
	}

	[Theory]
	[AutoData]
	public void And_NoneFailed_ShouldBeEmpty(
		TestError error1, TestError error2)
	{
		ITestResult result = Expect.That.Assemblies
			.ShouldSatisfy(Requirement.ForAssembly(_ => true, _ => error1)).And
			.ShouldSatisfy(Requirement.ForAssembly(_ => true, _ => error2))
			.Check.InExecutingAssembly();

		result.ShouldNotBeViolated();
	}

	[Theory]
	[AutoData]
	public void And_SecondFailed_ShouldIncludeOnlySecondError(
		TestError error1, TestError error2)
	{
		ITestResult result = Expect.That.Assemblies
			.ShouldSatisfy(Requirement.ForAssembly(_ => true, _ => error1)).And
			.ShouldSatisfy(Requirement.ForAssembly(_ => false, _ => error2))
			.Check.InExecutingAssembly();

		result.ShouldBeViolated();
		result.Errors[0].Should().Be(error2);
		result.Errors.Length.Should().Be(1);
	}

	[Theory]
	[AutoData]
	public void And_ShouldIncludeMultipleErrors(TestError error1, TestError error2)
	{
		ITestResult result = Expect.That.Assemblies
			.ShouldSatisfy(Requirement.ForAssembly(_ => false, _ => error1)).And
			.ShouldSatisfy(Requirement.ForAssembly(_ => false, _ => error2))
			.Check.InExecutingAssembly();

		result.ShouldBeViolated();
		result.Errors.Should().Contain(error1);
		result.Errors.Should().Contain(error2);
		result.Errors.Length.Should().Be(2);
	}

	[Theory]
	[AutoData]
	public void ToString_WithMultipleErrors_ShouldIncludeErrorCount(TestError error1,
		TestError error2)
	{
		ITestResult testResult = Expect.That.Assemblies
			.ShouldSatisfy(Requirement.ForAssembly(_ => false, _ => error1)).And
			.ShouldSatisfy(Requirement.ForAssembly(_ => false, _ => error2))
			.Check.InExecutingAssembly();

		string? result = testResult.ToString();

		result.Should().NotBeNull();
		result.Should().Contain("The rule is violated with 2 errors:" + Environment.NewLine);
		result.Should().Contain(" - " + error1.ToString()
			.Replace(Environment.NewLine, Environment.NewLine + "   "));
		result.Should().Contain(" - " + error2.ToString()
			.Replace(Environment.NewLine, Environment.NewLine + "   "));
	}

	[Theory]
	[AutoData]
	public void ToString_WithRuleNameAndWithError_ShouldIncludeRuleNameAndIsViolated(
		string ruleName)
	{
		ITestResult testResult = Expect.That.Assemblies
			.ShouldSatisfy(Requirement.ForAssembly(_ => false))
			.Check.InExecutingAssembly();

		string result = testResult.ToString(ruleName);

		result.Should().Contain($"The rule '{ruleName}' is violated:");
	}

	[Theory]
	[AutoData]
	public void ToString_WithRuleNameAndWithoutError_ShouldIncludeRuleNameAndIsNotViolated(
		string ruleName)
	{
		ITestResult testResult = Expect.That.Assemblies
			.ShouldSatisfy(Requirement.ForAssembly(_ => true))
			.Check.InExecutingAssembly();

		string result = testResult.ToString(ruleName);

		result.Should().Be($"The rule '{ruleName}' is not violated.");
	}
}
