using AutoFixture.Xunit2;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests;

public sealed class RequirementTests
{
	[Theory]
	[InlineAutoData(true, 0)]
	[InlineAutoData(false, 1)]
	public void Create_WithErrorGenerator_CollectErrors_ShouldAddExpectedErrorCount(
		bool predicateResult,
		int expectedErrorCount,
		TestError testError)
	{
		DummyClass element = new(1);
		List<TestError> errors = new();

		Requirement<DummyClass> sut =
			Requirement.Create<DummyClass>(_ => predicateResult, _ => testError);

		sut.CollectErrors(element, errors);
		errors.Count.Should().Be(expectedErrorCount);
		if (expectedErrorCount > 0)
		{
			errors[0].Should().Be(testError);
		}
	}

	[Theory]
	[InlineAutoData(true, 0)]
	[InlineAutoData(false, 1)]
	public void Delegate_WithErrorGenerator_CollectErrors_ShouldAddExpectedErrorCount(
		bool predicateResult,
		int expectedErrorCount,
		int multiply,
		TestError testError)
	{
		List<TestError> errors = new();

		Requirement<int> sut = Requirement.Delegate<int, DummyClass>(
			_ => Enumerable.Range(1, multiply).Select(x => new DummyClass(x)),
			Requirement.Create<DummyClass>(_ => predicateResult, _ => testError));

		sut.CollectErrors(5, errors);
		errors.Count.Should().Be(expectedErrorCount * multiply);
		errors.Should().AllBeEquivalentTo(testError);
	}

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
	public void ForConstructor_WithErrorGenerator_CollectErrors_ShouldAddExpectedErrorCount(
		bool predicateResult,
		int expectedErrorCount,
		TestError testError)
	{
		ConstructorInfo constructor = typeof(DummyClass).GetConstructors().First();
		List<TestError> errors = new();

		Requirement<ConstructorInfo> sut =
			Requirement.ForConstructor(_ => predicateResult, _ => testError);

		sut.CollectErrors(constructor, errors);
		errors.Count.Should().Be(expectedErrorCount);
		if (expectedErrorCount > 0)
		{
			errors[0].Should().Be(testError);
		}
	}

	[Theory]
	[InlineData(true, 0)]
	[InlineData(false, 1)]
	public void ForConstructor_WithExpression_CollectErrors_ShouldAddExpectedErrorCount(
		bool predicateResult,
		int expectedErrorCount)
	{
		ConstructorInfo constructor = typeof(DummyClass).GetConstructors().First();
		List<TestError> errors = new();

		Requirement<ConstructorInfo> sut = Requirement.ForConstructor(_ => predicateResult);

		sut.CollectErrors(constructor, errors);
		errors.Count.Should().Be(expectedErrorCount);
	}

	[Theory]
	[InlineAutoData(true, 0)]
	[InlineAutoData(false, 1)]
	public void ForEvent_WithErrorGenerator_CollectErrors_ShouldAddExpectedErrorCount(
		bool predicateResult,
		int expectedErrorCount,
		TestError testError)
	{
		EventInfo @event = typeof(DummyClass).GetEvents().First();
		List<TestError> errors = new();

		Requirement<EventInfo> sut = Requirement.ForEvent(_ => predicateResult, _ => testError);

		sut.CollectErrors(@event, errors);
		errors.Count.Should().Be(expectedErrorCount);
		if (expectedErrorCount > 0)
		{
			errors[0].Should().Be(testError);
		}
	}

	[Theory]
	[InlineData(true, 0)]
	[InlineData(false, 1)]
	public void ForEvent_WithExpression_CollectErrors_ShouldAddExpectedErrorCount(
		bool predicateResult,
		int expectedErrorCount)
	{
		EventInfo @event = typeof(DummyClass).GetEvents().First();
		List<TestError> errors = new();

		Requirement<EventInfo> sut = Requirement.ForEvent(_ => predicateResult);

		sut.CollectErrors(@event, errors);
		errors.Count.Should().Be(expectedErrorCount);
	}

	[Theory]
	[InlineAutoData(true, 0)]
	[InlineAutoData(false, 1)]
	public void ForField_WithErrorGenerator_CollectErrors_ShouldAddExpectedErrorCount(
		bool predicateResult,
		int expectedErrorCount,
		TestError testError)
	{
		FieldInfo field = typeof(DummyClass).GetFields().First();
		List<TestError> errors = new();

		Requirement<FieldInfo> sut = Requirement.ForField(_ => predicateResult, _ => testError);

		sut.CollectErrors(field, errors);
		errors.Count.Should().Be(expectedErrorCount);
		if (expectedErrorCount > 0)
		{
			errors[0].Should().Be(testError);
		}
	}

	[Theory]
	[InlineData(true, 0)]
	[InlineData(false, 1)]
	public void ForField_WithExpression_CollectErrors_ShouldAddExpectedErrorCount(
		bool predicateResult,
		int expectedErrorCount)
	{
		FieldInfo field = typeof(DummyClass).GetFields().First();
		List<TestError> errors = new();

		Requirement<FieldInfo> sut = Requirement.ForField(_ => predicateResult);

		sut.CollectErrors(field, errors);
		errors.Count.Should().Be(expectedErrorCount);
	}

	[Theory]
	[InlineAutoData(true, 0)]
	[InlineAutoData(false, 1)]
	public void ForMethod_WithErrorGenerator_CollectErrors_ShouldAddExpectedErrorCount(
		bool predicateResult,
		int expectedErrorCount,
		TestError testError)
	{
		MethodInfo method = typeof(DummyClass).GetDeclaredMethods().First();
		List<TestError> errors = new();

		Requirement<MethodInfo> sut = Requirement.ForMethod(_ => predicateResult, _ => testError);

		sut.CollectErrors(method, errors);
		errors.Count.Should().Be(expectedErrorCount);
		if (expectedErrorCount > 0)
		{
			errors[0].Should().Be(testError);
		}
	}

	[Theory]
	[InlineData(true, 0)]
	[InlineData(false, 1)]
	public void ForMethod_WithExpression_CollectErrors_ShouldAddExpectedErrorCount(
		bool predicateResult,
		int expectedErrorCount)
	{
		MethodInfo method = typeof(DummyClass).GetDeclaredMethods().First();
		List<TestError> errors = new();

		Requirement<MethodInfo> sut = Requirement.ForMethod(_ => predicateResult);

		sut.CollectErrors(method, errors);
		errors.Count.Should().Be(expectedErrorCount);
	}

	[Theory]
	[InlineAutoData(true, 0)]
	[InlineAutoData(false, 1)]
	public void ForProperty_WithErrorGenerator_CollectErrors_ShouldAddExpectedErrorCount(
		bool predicateResult,
		int expectedErrorCount,
		TestError testError)
	{
		PropertyInfo property = typeof(DummyClass).GetProperties().First();
		List<TestError> errors = new();

		Requirement<PropertyInfo> sut =
			Requirement.ForProperty(_ => predicateResult, _ => testError);

		sut.CollectErrors(property, errors);
		errors.Count.Should().Be(expectedErrorCount);
		if (expectedErrorCount > 0)
		{
			errors[0].Should().Be(testError);
		}
	}

	[Theory]
	[InlineData(true, 0)]
	[InlineData(false, 1)]
	public void ForProperty_WithExpression_CollectErrors_ShouldAddExpectedErrorCount(
		bool predicateResult,
		int expectedErrorCount)
	{
		PropertyInfo property = typeof(DummyClass).GetProperties().First();
		List<TestError> errors = new();

		Requirement<PropertyInfo> sut = Requirement.ForProperty(_ => predicateResult);

		sut.CollectErrors(property, errors);
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
