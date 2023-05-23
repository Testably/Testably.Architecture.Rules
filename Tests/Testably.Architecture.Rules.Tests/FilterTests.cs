using AutoFixture.Xunit2;
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
		IEventFilter constructorFilter = Have.Event;

		OnEventMock sut = new(constructorFilter);

		sut.And.Should().Be(constructorFilter);
	}

	[Theory]
	[InlineData(false)]
	[InlineData(true)]
	public void OnEvent_Applies_ShouldReturnPredicateResult(bool predicateResult)
	{
		IEventFilter constructorFilter = Have.Event;

		OnEventMock sut = new(constructorFilter, _ => predicateResult);

		sut.Applies(typeof(DummyClass).GetEvents().First()).Should().Be(predicateResult);
	}

	[Fact]
	public void OnField_And_ShouldReturnTypeFilter()
	{
		IFieldFilter constructorFilter = Have.Field;

		OnFieldMock sut = new(constructorFilter);

		sut.And.Should().Be(constructorFilter);
	}

	[Theory]
	[InlineData(false)]
	[InlineData(true)]
	public void OnField_Applies_ShouldReturnPredicateResult(bool predicateResult)
	{
		IFieldFilter constructorFilter = Have.Field;

		OnFieldMock sut = new(constructorFilter, _ => predicateResult);

		sut.Applies(typeof(DummyClass).GetFields().First()).Should().Be(predicateResult);
	}

	[Fact]
	public void OnMethod_And_ShouldReturnTypeFilter()
	{
		IMethodFilter constructorFilter = Have.Method;

		OnMethodMock sut = new(constructorFilter);

		sut.And.Should().Be(constructorFilter);
	}

	[Theory]
	[InlineData(false)]
	[InlineData(true)]
	public void OnMethod_Applies_ShouldReturnPredicateResult(bool predicateResult)
	{
		IMethodFilter constructorFilter = Have.Method;

		OnMethodMock sut = new(constructorFilter, _ => predicateResult);

		sut.Applies(typeof(DummyClass).GetMethods().First()).Should().Be(predicateResult);
	}

	[Fact]
	public void OnProperty_And_ShouldReturnTypeFilter()
	{
		IPropertyFilter constructorFilter = Have.Property;

		OnPropertyMock sut = new(constructorFilter);

		sut.And.Should().Be(constructorFilter);
	}

	[Theory]
	[InlineData(false)]
	[InlineData(true)]
	public void OnProperty_Applies_ShouldReturnPredicateResult(bool predicateResult)
	{
		IPropertyFilter constructorFilter = Have.Property;

		OnPropertyMock sut = new(constructorFilter, _ => predicateResult);

		sut.Applies(typeof(DummyClass).GetProperties().First()).Should().Be(predicateResult);
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
	} // ReSharper disable UnusedMember.Local
	private class DummyClass
	{
		public int? DummyProperty { get; set; }
		public int Value { get; }
		#pragma warning disable CS0414
		public int? DummyField = 0;
		#pragma warning restore CS0414

		public DummyClass(int value)
		{
			Value = value;
		}

		#pragma warning disable CS0067
		public event Dummy DummyEvent = null!;
		#pragma warning restore CS0067
		public delegate void Dummy();

		public void DummyMethod() { }
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
}
