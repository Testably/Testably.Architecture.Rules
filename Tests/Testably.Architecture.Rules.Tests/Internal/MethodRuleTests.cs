﻿using AutoFixture.Xunit2;
using FluentAssertions;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Internal;

public sealed class MethodRuleTests
{
	[Fact]
	public void ShouldSatisfy_DefaultError_ShouldIncludeMethodInfoName()
	{
		MethodInfo methodInfo = typeof(Dummy).GetMethods().First();
		string expectedMethodInfoName = $"'{methodInfo.Name}'";
		IRule rule = Expect.That.Methods
			.Which(t => t == methodInfo)
			.ShouldSatisfy(_ => false);

		ITestResult result = rule.Check
			.In(typeof(Dummy).Assembly);

		TestError error = result.Errors.Single();
		error.ToString().Should().Contain(expectedMethodInfoName);
	}

	[Theory]
	[AutoData]
	public void ShouldSatisfy_False_ShouldIncludeError(TestError error)
	{
		MethodInfo methodInfo = typeof(Dummy).GetMethods().First();
		IRule rule = Expect.That.Methods
			.Which(t => t == methodInfo)
			.ShouldSatisfy(Requirement.ForMethod(_ => false, _ => error));

		ITestResult result = rule.Check
			.In(typeof(Dummy).Assembly);

		result.Errors.Should().NotBeEmpty();
		result.Errors.Single().Should().Be(error);
	}

	[Theory]
	[AutoData]
	public void ShouldSatisfy_True_ShouldNotIncludeError(TestError error)
	{
		MethodInfo methodInfo = typeof(Dummy).GetMethods().First();
		IRule rule = Expect.That.Methods
			.Which(t => t == methodInfo)
			.ShouldSatisfy(Requirement.ForMethod(_ => true, _ => error));

		ITestResult result = rule.Check
			.In(typeof(Dummy).Assembly);

		result.Errors.Should().BeEmpty();
	}

	[Fact]
	public void Which_ShouldFilterOutMethodInfos()
	{
		int allMethodsCount = typeof(Dummy).GetMethods().Length;

		IRule rule = Expect.That.Methods
			.Which(t => t.DeclaringType == typeof(Dummy)).And
			.Which(p => p.Name.StartsWith(nameof(Dummy.Method1)))
			.ShouldSatisfy(Requirement.ForMethod(_ => false));

		ITestResult result = rule.Check
			.In(typeof(Dummy).Assembly);

		result.Errors.Length.Should().BeLessThan(allMethodsCount);
		result.Errors.Should().OnlyContain(e => e.ToString().Contains($"'{nameof(Dummy.Method1)}"));
	}

	private class Dummy
	{
		public void Method1()
		{
		}

		// ReSharper disable once UnusedMember.Local
		public void Method2()
		{
		}
	}
}