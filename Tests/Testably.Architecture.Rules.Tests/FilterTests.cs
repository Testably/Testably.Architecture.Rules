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
		DummyFooClass test = new(value + valueOffset);
		Filter<DummyFooClass> sut = Filter.FromPredicate<DummyFooClass>(d => d.Value == value);

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
		DummyFooClass test = new(value);
		Filter<DummyFooClass> sut = Filter.FromPredicate<DummyFooClass>(_ => predicateResult, name);

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
	[InlineData(false, false, false)]
	[InlineData(true, false, true)]
	[InlineData(false, true, true)]
	[InlineData(true, true, true)]
	public void OnConstructor_Applies_ShouldUseAnyForResult(bool result1, bool result2, bool expectedResult)
	{
		IConstructorFilter constructorFilter = Have.Constructor;

		OnConstructorMock sut = new(constructorFilter);
		sut.AddPredicates(
			Filter.FromPredicate<ConstructorInfo>(_ => result1),
			Filter.FromPredicate<ConstructorInfo>(_ => result2));

		sut.Applies(typeof(DummyFooClass).GetConstructors().First()).Should().Be(expectedResult);
	}

	[Theory]
	[AutoData]
	public void OnConstructor_Or_ToString_ShouldIncludeAllFilterNames(
		string filter1,
		string filter2)
	{
		IConstructorFilter constructorFilter = Have.Constructor;

		OnConstructorMock sut = new(constructorFilter);
		sut.AddPredicates(
			Filter.FromPredicate<ConstructorInfo>(_ => false, filter1),
			Filter.FromPredicate<ConstructorInfo>(_ => true, filter2));

		string result = sut.ToString();

		result.Should().Be($"{filter1} or {filter2}");
	}

	[Fact]
	public void OnEvent_And_ShouldReturnTypeFilter()
	{
		IEventFilter eventFilter = Have.Event;

		OnEventMock sut = new(eventFilter);

		sut.And.Should().Be(eventFilter);
	}

	[Theory]
	[InlineData(false, false, false)]
	[InlineData(true, false, true)]
	[InlineData(false, true, true)]
	[InlineData(true, true, true)]
	public void OnEvent_Applies_ShouldUseAnyForResult(bool result1, bool result2, bool expectedResult)
	{
		IEventFilter eventFilter = Have.Event;

		OnEventMock sut = new(eventFilter);
		sut.AddPredicates(
			Filter.FromPredicate<EventInfo>(_ => result1),
			Filter.FromPredicate<EventInfo>(_ => result2));

		sut.Applies(typeof(DummyFooClass).GetEvents().First()).Should().Be(expectedResult);
	}

	[Theory]
	[AutoData]
	public void OnEvent_Or_ToString_ShouldIncludeAllFilterNames(
		string filter1,
		string filter2)
	{
		IEventFilter eventFilter = Have.Event;

		OnEventMock sut = new(eventFilter);
		sut.AddPredicates(
			Filter.FromPredicate<EventInfo>(_ => false, filter1),
			Filter.FromPredicate<EventInfo>(_ => true, filter2));

		string result = sut.ToString();

		result.Should().Be($"{filter1} or {filter2}");
	}

	[Fact]
	public void OnField_And_ShouldReturnTypeFilter()
	{
		IFieldFilter fieldFilter = Have.Field;

		OnFieldMock sut = new(fieldFilter);

		sut.And.Should().Be(fieldFilter);
	}

	[Theory]
	[InlineData(false, false, false)]
	[InlineData(true, false, true)]
	[InlineData(false, true, true)]
	[InlineData(true, true, true)]
	public void OnField_Applies_ShouldUseAnyForResult(bool result1, bool result2, bool expectedResult)
	{
		IFieldFilter fieldFilter = Have.Field;

		OnFieldMock sut = new(fieldFilter);
		sut.AddPredicates(
			Filter.FromPredicate<FieldInfo>(_ => result1),
			Filter.FromPredicate<FieldInfo>(_ => result2));

		sut.Applies(typeof(DummyFooClass).GetFields().First()).Should().Be(expectedResult);
	}

	[Theory]
	[AutoData]
	public void OnField_Or_ToString_ShouldIncludeAllFilterNames(
		string filter1,
		string filter2)
	{
		IFieldFilter fieldFilter = Have.Field;

		OnFieldMock sut = new(fieldFilter);
		sut.AddPredicates(
			Filter.FromPredicate<FieldInfo>(_ => false, filter1),
			Filter.FromPredicate<FieldInfo>(_ => true, filter2));

		string result = sut.ToString();

		result.Should().Be($"{filter1} or {filter2}");
	}

	[Fact]
	public void OnMethod_And_ShouldReturnTypeFilter()
	{
		IMethodFilter methodFilter = Have.Method;

		OnMethodMock sut = new(methodFilter);

		sut.And.Should().Be(methodFilter);
	}

	[Theory]
	[InlineData(false, false, false)]
	[InlineData(true, false, true)]
	[InlineData(false, true, true)]
	[InlineData(true, true, true)]
	public void OnMethod_Applies_ShouldUseAnyForResult(bool result1, bool result2, bool expectedResult)
	{
		IMethodFilter methodFilter = Have.Method;

		OnMethodMock sut = new(methodFilter);
		sut.AddPredicates(
			Filter.FromPredicate<MethodInfo>(_ => result1),
			Filter.FromPredicate<MethodInfo>(_ => result2));

		sut.Applies(typeof(DummyFooClass).GetMethods().First()).Should().Be(expectedResult);
	}

	[Theory]
	[AutoData]
	public void OnMethod_Or_ToString_ShouldIncludeAllFilterNames(
		string filter1,
		string filter2)
	{
		IMethodFilter methodFilter = Have.Method;

		OnMethodMock sut = new(methodFilter);
		sut.AddPredicates(
			Filter.FromPredicate<MethodInfo>(_ => false, filter1),
			Filter.FromPredicate<MethodInfo>(_ => true, filter2));

		string result = sut.ToString();

		result.Should().Be($"{filter1} or {filter2}");
	}

	[Fact]
	public void OnParameter_And_ShouldReturnTypeFilter()
	{
		IParameterFilter<IUnorderedParameterFilterResult> parameterFilter = Parameters.Any;

		OnParameterMock sut = new(parameterFilter);

		sut.And.Should().Be(parameterFilter);
	}

	[Theory]
	[InlineData(false, false, false)]
	[InlineData(true, false, true)]
	[InlineData(false, true, true)]
	[InlineData(true, true, true)]
	public void OnParameter_Applies_ShouldUseAnyForResult(bool result1, bool result2, bool expectedResult)
	{
		var parameter = typeof(DummyFooClass).GetConstructors().First().GetParameters()[0];
		IParameterFilter<IUnorderedParameterFilterResult> parameterFilter = Parameters.Any;

		OnParameterMock sut = new(parameterFilter);
		sut.AddPredicates(
			Filter.FromPredicate<ParameterInfo>(_ => result1),
			Filter.FromPredicate<ParameterInfo>(_ => result2));

		sut.Applies(parameter).Should().Be(expectedResult);
	}

	[Theory]
	[AutoData]
	public void OnParameter_Or_ToString_ShouldIncludeAllFilterNames(
		string filter1,
		string filter2)
	{
		IParameterFilter<IUnorderedParameterFilterResult> parameterFilter = Parameters.Any;

		OnParameterMock sut = new(parameterFilter);
		sut.AddPredicates(
			Filter.FromPredicate<ParameterInfo>(_ => false, filter1),
			Filter.FromPredicate<ParameterInfo>(_ => true, filter2));

		string result = sut.ToString();

		result.Should().Be($"{filter1} or {filter2}");
	}

	[Fact]
	public void OnProperty_And_ShouldReturnTypeFilter()
	{
		IPropertyFilter propertyFilter = Have.Property;

		OnPropertyMock sut = new(propertyFilter);

		sut.And.Should().Be(propertyFilter);
	}

	[Theory]
	[InlineData(false, false, false)]
	[InlineData(true, false, true)]
	[InlineData(false, true, true)]
	[InlineData(true, true, true)]
	public void OnProperty_Applies_ShouldUseAnyForResult(bool result1, bool result2, bool expectedResult)
	{
		IPropertyFilter propertyFilter = Have.Property;

		OnPropertyMock sut = new(propertyFilter);
		sut.AddPredicates(
			Filter.FromPredicate<PropertyInfo>(_ => result1),
			Filter.FromPredicate<PropertyInfo>(_ => result2));

		sut.Applies(typeof(DummyFooClass).GetProperties().First()).Should().Be(expectedResult);
	}

	[Theory]
	[AutoData]
	public void OnProperty_Or_ToString_ShouldIncludeAllFilterNames(
		string filter1,
		string filter2)
	{
		IPropertyFilter propertyFilter = Have.Property;

		OnPropertyMock sut = new(propertyFilter);
		sut.AddPredicates(
			Filter.FromPredicate<PropertyInfo>(_ => false, filter1),
			Filter.FromPredicate<PropertyInfo>(_ => true, filter2));

		string result = sut.ToString();

		result.Should().Be($"{filter1} or {filter2}");
	}

	[Fact]
	public void OnType_And_ShouldReturnTypeFilter()
	{
		ITypeFilter typeFilter = Expect.That.Types;

		OnTypeMock sut = new(typeFilter);
		sut.AddPredicates(Filter.FromPredicate<Type>(_ => true));

		sut.And.Should().Be(typeFilter);
	}

	[Theory]
	[InlineData(false, false, false)]
	[InlineData(true, false, true)]
	[InlineData(false, true, true)]
	[InlineData(true, true, true)]
	public void OnType_Applies_ShouldUseAnyForResult(bool result1, bool result2, bool expectedResult)
	{
		ITypeFilter typeFilter = Expect.That.Types;

		OnTypeMock sut = new(typeFilter);
		sut.AddPredicates(
			Filter.FromPredicate<Type>(_ => result1),
			Filter.FromPredicate<Type>(_ => result2));

		sut.Applies(GetType()).Should().Be(expectedResult);
	}

	[Fact]
	public void OnType_Assemblies_ShouldExcludeAssembliesFromOtherTypes()
	{
		ITypeFilter typeFilter = Expect.That.Types;

		OnTypeMock sut = new(typeFilter);
		sut.AddPredicates(Filter.FromPredicate<Type>(t => t == typeof(FilterTests)));
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

		OnTypeMock sut = new(typeFilter);
		sut.AddPredicates(Filter.FromPredicate<Type>(t => t == typeof(FilterTests)));
		IAssemblyExpectation assemblies = sut.Assemblies;

		ITestResult result = assemblies
			.ShouldSatisfy(_ => false)
			.AllowEmpty()
			.Check.InAssemblyContaining<FilterTests>();

		result.ShouldBeViolated();
	}

	private class OnConstructorMock : Filter.OnConstructor
	{
		public OnConstructorMock(IConstructorFilter constructorFilter)
			: base(constructorFilter)
		{
		}

		public void AddPredicates(params Filter<ConstructorInfo>[] predicates)
		{
			Predicates.AddRange(predicates);
		}
	}

	private class OnEventMock : Filter.OnEvent
	{
		public OnEventMock(IEventFilter eventFilter)
			: base(eventFilter)
		{
		}

		public void AddPredicates(params Filter<EventInfo>[] predicates)
		{
			Predicates.AddRange(predicates);
		}
	}

	private class OnFieldMock : Filter.OnField
	{
		public OnFieldMock(IFieldFilter fieldFilter)
			: base(fieldFilter)
		{
		}

		public void AddPredicates(params Filter<FieldInfo>[] predicates)
		{
			Predicates.AddRange(predicates);
		}
	}

	private class OnMethodMock : Filter.OnMethod
	{
		public OnMethodMock(IMethodFilter methodFilter)
			: base(methodFilter)
		{
		}

		public void AddPredicates(params Filter<MethodInfo>[] predicates)
		{
			Predicates.AddRange(predicates);
		}
	}

	private class OnParameterMock : Filter.OnParameter<IUnorderedParameterFilterResult>
	{
		public OnParameterMock(
			IParameterFilter<IUnorderedParameterFilterResult> parameterFilter)
			: base(parameterFilter)
		{
		}

		/// <inheritdoc cref="IParameterFilterResult{TResult}.FriendlyName()" />
		public override string FriendlyName()
			=> throw new NotSupportedException();

		public void AddPredicates(params Filter<ParameterInfo>[] predicates)
		{
			Predicates.AddRange(predicates);
		}
	}

	private class OnPropertyMock : Filter.OnProperty
	{
		public OnPropertyMock(IPropertyFilter propertyFilter)
			: base(propertyFilter)
		{
		}

		public void AddPredicates(params Filter<PropertyInfo>[] predicates)
		{
			Predicates.AddRange(predicates);
		}
	}

	private class OnTypeMock : Filter.OnType
	{
		public OnTypeMock(ITypeFilter typeFilter)
			: base(typeFilter)
		{
		}

		public void AddPredicates(params Filter<Type>[] predicates)
		{
			Predicates.AddRange(predicates);
		}
	}
}
