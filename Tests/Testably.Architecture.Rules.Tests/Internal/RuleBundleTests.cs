using AutoFixture.Xunit2;
using FluentAssertions;
using System;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Internal;

public sealed class RuleBundleTests
{
	[Theory]
	[AutoData]
	public void Check_WithViolatedRules_ShouldIncludeNameInErrorMessage(string bundleName)
	{
		IRule rule = Rules.Bundle(bundleName,
			Expect.That.Assemblies.ShouldSatisfy(_ => false),
			Expect.That.Assemblies.ShouldSatisfy(_ => false));

		ITestResult result = rule.Check
			.InExecutingAssembly();

		result.ShouldBeViolated();
		result.ToString().Should().Contain($"The rules '{bundleName}' are violated with 2 errors:");
	}

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
	public void Check_WithViolatedRules_ShouldIncludeTestErrorsInErrorMessage(
		string bundleName,
		TestError error1,
		TestError error2)
	{
		IRule rule = Rules.Bundle(bundleName,
			Expect.That.Assemblies.ShouldSatisfy(
				Requirement.ForAssembly(_ => false, _ => error1)),
			Expect.That.Assemblies.ShouldSatisfy(
				Requirement.ForAssembly(_ => false, _ => error2)));
		string expectedErrorString =
			$" - {error1.ToString().Replace(Environment.NewLine, Environment.NewLine + "   ")}{Environment.NewLine}" +
			$" - {error2.ToString().Replace(Environment.NewLine, Environment.NewLine + "   ")}";

		ITestResult result = rule.Check
			.InExecutingAssembly();

		result.ShouldBeViolated();
		result.ToString().Should().Contain($"The rules '{bundleName}' are violated with 2 errors:" +
		                                   Environment.NewLine);
		result.ToString().Should().Contain(expectedErrorString);
	}
}
