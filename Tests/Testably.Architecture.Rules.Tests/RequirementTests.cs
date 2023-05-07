using AutoFixture.Xunit2;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace Testably.Architecture.Rules.Tests;

public sealed class RequirementTests
{
	[Theory]
	[InlineAutoData(true, 0)]
	[InlineAutoData(false, 1)]
	public void ForAssembly_WithErrorGenerator_CollectErrors_ShouldAddExpectedErrorCount(
		bool predicateResult,
		int expectedErrorCount,
		TestError testError)
	{
		Assembly assembly = Assembly.GetExecutingAssembly();
		List<TestError> errors = new();

		Requirement<Assembly> sut = Requirement.ForAssembly(_ => predicateResult, _ => testError);

		sut.CollectErrors(assembly, errors);
		errors.Count.Should().Be(expectedErrorCount);
		if (expectedErrorCount > 0)
		{
			errors[0].Should().Be(testError);
		}
	}

	[Theory]
	[InlineData(true, 0)]
	[InlineData(false, 1)]
	public void ForAssembly_WithExpression_CollectErrors_ShouldAddExpectedErrorCount(
		bool predicateResult,
		int expectedErrorCount)
	{
		Assembly assembly = Assembly.GetExecutingAssembly();
		List<TestError> errors = new();

		Requirement<Assembly> sut = Requirement.ForAssembly(_ => predicateResult);

		sut.CollectErrors(assembly, errors);
		errors.Count.Should().Be(expectedErrorCount);
	}

	[Theory]
	[InlineAutoData(true, 0)]
	[InlineAutoData(false, 1)]
	public void ForType_WithErrorGenerator_CollectErrors_ShouldAddExpectedErrorCount(
		bool predicateResult,
		int expectedErrorCount,
		TestError testError)
	{
		Type type = typeof(RequirementTests);
		List<TestError> errors = new();

		Requirement<Type> sut = Requirement.ForType(_ => predicateResult, _ => testError);

		sut.CollectErrors(type, errors);
		errors.Count.Should().Be(expectedErrorCount);
		if (expectedErrorCount > 0)
		{
			errors[0].Should().Be(testError);
		}
	}

	[Theory]
	[InlineData(true, 0)]
	[InlineData(false, 1)]
	public void ForType_WithExpression_CollectErrors_ShouldAddExpectedErrorCount(
		bool predicateResult,
		int expectedErrorCount)
	{
		Type type = typeof(RequirementTests);
		List<TestError> errors = new();

		Requirement<Type> sut = Requirement.ForType(_ => predicateResult);

		sut.CollectErrors(type, errors);
		errors.Count.Should().Be(expectedErrorCount);
	}
}
