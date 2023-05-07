using AutoFixture.Xunit2;
using FluentAssertions;
using System;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests;

public sealed class FilterTests
{
	[Theory]
	[InlineAutoData(0, true)]
	[InlineAutoData(1, false)]
	public void FromPredicate_WithExpression_NameShouldContainExpression(
		int valueOffset,
		bool predicateResult,
		int value)
	{
		DummyClass test = new(value + valueOffset);
		Filter<DummyClass> sut = Filter.FromPredicate<DummyClass>(d => d.Value == value);

		bool result = sut.Applies(test);
		result.Should().Be(predicateResult);
		sut.ToString().Should().Contain("d.Value")
			.And.Contain("value");
	}

	[Theory]
	[InlineAutoData(true)]
	[InlineAutoData(false)]
	public void FromPredicate_WithName_Applies_ShouldReturnPredicateResult(
		bool predicateResult,
		string name,
		int value)
	{
		DummyClass test = new(value);
		Filter<DummyClass> sut = Filter.FromPredicate<DummyClass>(_ => predicateResult, name);

		bool result = sut.Applies(test);
		result.Should().Be(predicateResult);
		sut.ToString().Should().Be(name);
	}

	[Fact]
	public void OnType_And_ShouldReturnTypeFilter()
	{
		ITypeFilter typeFilter = Expect.That.Types;

		OnTypeMock sut = new(typeFilter, _ => true);

		sut.And.Should().Be(typeFilter);
	}

	[Theory]
	[InlineData(false)]
	[InlineData(true)]
	public void OnType_Applies_ShouldReturnPredicateResult(bool predicateResult)
	{
		ITypeFilter typeFilter = Expect.That.Types;

		OnTypeMock sut = new(typeFilter, _ => predicateResult);

		sut.Applies(GetType()).Should().Be(predicateResult);
	}

	[Fact]
	public void OnType_Assemblies_ShouldExcludeAssembliesFromOtherTypes()
	{
		ITypeFilter typeFilter = Expect.That.Types;

		OnTypeMock sut = new(typeFilter, t => t == typeof(FilterTests));
		IAssemblyExpectation assemblies = sut.Assemblies;

		ITestResult result = assemblies
			.ShouldSatisfy(_ => false)
			.AllowEmpty()
			.Check.InAssemblyContaining<Filter.OnType>();

		result.ShouldNotBeViolated();
	}

	[Fact]
	public void OnType_Assemblies_ShouldIncludeAssembliesFromType()
	{
		ITypeFilter typeFilter = Expect.That.Types;

		OnTypeMock sut = new(typeFilter, t => t == typeof(FilterTests));
		IAssemblyExpectation assemblies = sut.Assemblies;

		ITestResult result = assemblies
			.ShouldSatisfy(_ => false)
			.AllowEmpty()
			.Check.InAssemblyContaining<FilterTests>();

		result.ShouldBeViolated();
	}

	private class DummyClass
	{
		public int Value { get; }

		public DummyClass(int value)
		{
			Value = value;
		}
	}

	private class OnTypeMock : Filter.OnType
	{
		public OnTypeMock(
			ITypeFilter typeFilter,
			Func<Type, bool> predicate)
			: base(typeFilter, predicate)
		{
		}
	}
}
