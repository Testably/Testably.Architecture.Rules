﻿using AutoFixture.Xunit2;
using FluentAssertions;
using System;
using System.Linq;
using System.Reflection;
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
	public void OnConstructor_And_ShouldReturnTypeFilter()
	{
		IConstructorFilter constructorFilter = Have.Constructor;

		OnConstructorMock sut = new(constructorFilter);

		sut.And.Should().Be(constructorFilter);
	}

	[Theory]
	[InlineData(false)]
	[InlineData(true)]
	public void OnConstructor_Applies_ShouldReturnPredicateResult(bool predicateResult)
	{
		IConstructorFilter constructorFilter = Have.Constructor;

		OnConstructorMock sut = new(constructorFilter, _ => predicateResult);

		sut.Applies(typeof(DummyClass).GetConstructors().First()).Should().Be(predicateResult);
	}

	[Fact]
	public void OnEvent_And_ShouldReturnTypeFilter()
	{
		IEventFilter eventFilter = Have.Event;

		OnEventMock sut = new(eventFilter);

		sut.And.Should().Be(eventFilter);
	}

	[Theory]
	[InlineData(false)]
	[InlineData(true)]
	public void OnEvent_Applies_ShouldReturnPredicateResult(bool predicateResult)
	{
		IEventFilter eventFilter = Have.Event;

		OnEventMock sut = new(eventFilter, _ => predicateResult);

		sut.Applies(typeof(DummyClass).GetEvents().First()).Should().Be(predicateResult);
	}

	[Fact]
	public void OnField_And_ShouldReturnTypeFilter()
	{
		IFieldFilter fieldFilter = Have.Field;

		OnFieldMock sut = new(fieldFilter);

		sut.And.Should().Be(fieldFilter);
	}

	[Theory]
	[InlineData(false)]
	[InlineData(true)]
	public void OnField_Applies_ShouldReturnPredicateResult(bool predicateResult)
	{
		IFieldFilter fieldFilter = Have.Field;

		OnFieldMock sut = new(fieldFilter, _ => predicateResult);

		sut.Applies(typeof(DummyClass).GetFields().First()).Should().Be(predicateResult);
	}

	[Fact]
	public void OnMethod_And_ShouldReturnTypeFilter()
	{
		IMethodFilter methodFilter = Have.Method;

		OnMethodMock sut = new(methodFilter);

		sut.And.Should().Be(methodFilter);
	}

	[Theory]
	[InlineData(false)]
	[InlineData(true)]
	public void OnMethod_Applies_ShouldReturnPredicateResult(bool predicateResult)
	{
		IMethodFilter methodFilter = Have.Method;

		OnMethodMock sut = new(methodFilter, _ => predicateResult);

		sut.Applies(typeof(DummyClass).GetMethods().First()).Should().Be(predicateResult);
	}

	[Fact]
	public void OnProperty_And_ShouldReturnTypeFilter()
	{
		IPropertyFilter propertyFilter = Have.Property;

		OnPropertyMock sut = new(propertyFilter);

		sut.And.Should().Be(propertyFilter);
	}

	[Theory]
	[InlineData(false)]
	[InlineData(true)]
	public void OnProperty_Applies_ShouldReturnPredicateResult(bool predicateResult)
	{
		IPropertyFilter propertyFilter = Have.Property;

		OnPropertyMock sut = new(propertyFilter, _ => predicateResult);

		sut.Applies(typeof(DummyClass).GetProperties().First()).Should().Be(predicateResult);
	}

	[Fact]
	public void OnParameter_And_ShouldReturnTypeFilter()
	{
		IParameterFilter<IUnorderedParameterFilterResult> parameterFilter = Parameters.Any;

		OnParameterMock sut = new(parameterFilter);

		sut.And.Should().Be(parameterFilter);
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

	private class OnConstructorMock : Filter.OnConstructor
	{
		public OnConstructorMock(
			IConstructorFilter constructorFilter,
			Func<ConstructorInfo, bool>? predicate = null)
			: base(constructorFilter)
		{
			if (predicate != null)
			{
				Predicates.Add(Filter.FromPredicate(predicate, "predicate"));
			}
		}
	}

	private class OnEventMock : Filter.OnEvent
	{
		public OnEventMock(
			IEventFilter constructorFilter,
			Func<EventInfo, bool>? predicate = null)
			: base(constructorFilter)
		{
			if (predicate != null)
			{
				Predicates.Add(Filter.FromPredicate(predicate, "predicate"));
			}
		}
	}

	private class OnFieldMock : Filter.OnField
	{
		public OnFieldMock(
			IFieldFilter constructorFilter,
			Func<FieldInfo, bool>? predicate = null)
			: base(constructorFilter)
		{
			if (predicate != null)
			{
				Predicates.Add(Filter.FromPredicate(predicate, "predicate"));
			}
		}
	}

	private class OnMethodMock : Filter.OnMethod
	{
		public OnMethodMock(
			IMethodFilter constructorFilter,
			Func<MethodInfo, bool>? predicate = null)
			: base(constructorFilter)
		{
			if (predicate != null)
			{
				Predicates.Add(Filter.FromPredicate(predicate, "predicate"));
			}
		}
	}

	private class OnPropertyMock : Filter.OnProperty
	{
		public OnPropertyMock(
			IPropertyFilter constructorFilter,
			Func<PropertyInfo, bool>? predicate = null)
			: base(constructorFilter)
		{
			if (predicate != null)
			{
				Predicates.Add(Filter.FromPredicate(predicate, "predicate"));
			}
		}
	}

	private class OnParameterMock : Filter.OnParameter<IUnorderedParameterFilterResult>
	{
		public OnParameterMock(
			IParameterFilter<IUnorderedParameterFilterResult> constructorFilter)
			: base(constructorFilter)
		{
		}

		/// <inheritdoc cref="Filter{ParameterInfo}.Applies(ParameterInfo)" />
		public override bool Applies(ParameterInfo type)
			=> throw new NotSupportedException();

		/// <inheritdoc cref="IParameterFilterResult{TResult}.FriendlyName()" />
		public override string FriendlyName()
			=> throw new NotSupportedException();
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
