using AutoFixture.Xunit2;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Internal;

public sealed class RuleBundleTests
{
	[Theory]
	[AutoData]
	public void Check_WithoutViolatedRules_ShouldNotBeViolated(string bundleName)
	{
		IRule rule = Rules.Bundle(bundleName,
			Expect.That.Assemblies.ShouldSatisfy(_ => true),
			Expect.That.Assemblies.ShouldSatisfy(_ => true));

		ITestResult result = rule.Check
			.InExecutingAssembly();

		result.ShouldNotBeViolated();
		result.ToString().Should().Contain($"The rules '{bundleName}' are not violated.");
	}

	[Theory]
	[AutoData]
	public void Check_WithViolatedRules_ShouldIncludeNameInErrorMessage(string bundleName)
	{
		IRule rule = Rules.Bundle(bundleName,
			Expect.That.Assemblies.ShouldSatisfy(_ => false));

		ITestResult result = rule.Check
			.InExecutingAssembly();

		result.ShouldBeViolated();
		result.ToString().Should().Contain($"The rules '{bundleName}' are violated:");
	}

	[Theory]
	[AutoData]
	public void Check_WithViolatedRules_ShouldIncludeTestErrorsInErrorMessage(
		string bundleName,
		TestError error1,
		TestError error2)
	{
		TestError multiLineError = new($"foo{Environment.NewLine}bar");
		IRule rule = Rules.Bundle(bundleName,
			Expect.That.Assemblies.ShouldSatisfy(
				Requirement.ForAssembly(_ => false, _ => error1)),
			Expect.That.Assemblies.ShouldSatisfy(
				Requirement.ForAssembly(_ => false, _ => error2)),
			Expect.That.Assemblies.ShouldSatisfy(
				Requirement.ForAssembly(_ => false, _ => multiLineError)));
		string expectedErrorString =
			$" - {error1.ToString().Replace(Environment.NewLine, Environment.NewLine + "   ")}{Environment.NewLine}" +
			$" - {error2.ToString().Replace(Environment.NewLine, Environment.NewLine + "   ")}";

		ITestResult result = rule.Check
			.InExecutingAssembly();

		result.ShouldBeViolated();
		result.ToString().Should().Contain($"The rules '{bundleName}' are violated with 3 errors:" +
		                                   Environment.NewLine);
		result.ToString().Should().Contain(expectedErrorString);
		result.ToString().Should().Contain(" - " + multiLineError.ToString()
			.Replace(Environment.NewLine, Environment.NewLine + "   "));
	}

	[Theory]
	[AutoData]
	public void WithDescription_ShouldBeContainedInToString(
		string bundleName,
		string description)
	{
		IRule rule = Rules.Bundle(bundleName,
			Expect.That.Assemblies.ShouldSatisfy(_ => false),
			Expect.That.Assemblies.ShouldSatisfy(_ => false));

		ITestResult result = rule.Check
			.InExecutingAssembly();

		result.WithDescription(description).ShouldBeViolated();
		result.ToString().Should().Contain($"({description})");
	}

	[Theory]
	[AutoData]
	public void WithLog_ShouldContainErrorMessage(string bundleName, string errorMessage)
	{
		List<string> receivedLogs = new();
		IRule rule = Rules.Bundle(bundleName,
			Expect.That.Assemblies.ShouldSatisfy(
				Requirement.ForAssembly(
					_ => false,
					_ => new TestError(errorMessage))));

		ITestResult result = rule.Check
			.WithLog(m => receivedLogs.Add(m))
			.InExecutingAssembly();

		result.ShouldBeViolated();
		receivedLogs.Should().Contain(l => l.Contains(errorMessage));
	}
}
