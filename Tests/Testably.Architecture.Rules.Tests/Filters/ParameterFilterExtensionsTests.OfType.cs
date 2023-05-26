using FluentAssertions;
using System;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Filters;

public sealed partial class ParameterFilterExtensionsTests
{
	public sealed class OfTypeTests
	{
		[Theory]
		[InlineData(true, true)]
		[InlineData(false, false)]
		public void OfType_Ordered_FooBase_ShouldReturnExpectedValue(bool allowDerivedType,
			bool expectedValue)
		{
			IParameterFilter<IOrderedParameterFilterResult> sut = Parameters.InOrder;

			Filter<Type> typeFilter = Have.Method
				.With(
					sut.OfType<FooBase>(allowDerivedType))
				.ToTypeFilter();

			bool result = typeFilter.Applies(typeof(TestClass));
			result.Should().Be(expectedValue);
		}

		[Fact]
		public void OfType_Ordered_NotMatchingType_ShouldReturnFalse()
		{
			IParameterFilter<IOrderedParameterFilterResult> sut = Parameters.InOrder;

			Filter<Type> typeFilter = Have.Method
				.With(
					sut.OfType<Bar>())
				.ToTypeFilter();

			bool result = typeFilter.Applies(typeof(TestClass));
			result.Should().BeFalse();
		}

		[Fact]
		public void OfType_Ordered_OrOfType_NotMatchingAndMatchingType_ShouldReturnTrue()
		{
			IParameterFilter<IOrderedParameterFilterResult> sut = Parameters.InOrder;

			Filter<Type> typeFilter = Have.Method
				.With(
					sut.OfType<Bar>().OrOfType<Foo>())
				.ToTypeFilter();

			bool result = typeFilter.Applies(typeof(TestClass));
			result.Should().BeTrue();
		}

		[Theory]
		[InlineData(typeof(Foo), true)]
		[InlineData(typeof(Bar), false)]
		public void OfType_Ordered_ShouldReturnExpectedValue(Type type, bool expectedValue)
		{
			IParameterFilter<IOrderedParameterFilterResult> sut = Parameters.InOrder;

			Filter<Type> typeFilter = Have.Method
				.With(
					sut.OfType(type))
				.ToTypeFilter();

			bool result = typeFilter.Applies(typeof(TestClass));
			result.Should().Be(expectedValue);
		}

		[Theory]
		[InlineData(typeof(Foo), typeof(Bar), true)]
		[InlineData(typeof(Bar), typeof(Foo), false)]
		public void OfType_Ordered_Then_ShouldGoToNextParameter(Type type1, Type type2,
			bool expectedValue)
		{
			IParameterFilter<IOrderedParameterFilterResult> sut = Parameters.InOrder;

			Filter<Type> typeFilter = Have.Method
				.With(sut
					.OfType(type1)
					.Then()
					.OfType(type2))
				.ToTypeFilter();

			bool result = typeFilter.Applies(typeof(TestClassWithMultipleParameters));
			result.Should().Be(expectedValue);
		}

		[Theory]
		[InlineData(true, true)]
		[InlineData(false, false)]
		public void OfType_Unordered_FooBase_ShouldReturnExpectedValue(bool allowDerivedType,
			bool expectedValue)
		{
			IParameterFilter<IUnorderedParameterFilterResult> sut = Parameters.Any;

			Filter<Type> typeFilter = Have.Method
				.With(
					sut.OfType<FooBase>(allowDerivedType))
				.ToTypeFilter();

			bool result = typeFilter.Applies(typeof(TestClass));
			result.Should().Be(expectedValue);
		}

		[Fact]
		public void OfType_Unordered_NotMatchingType_ShouldReturnFalse()
		{
			IParameterFilter<IUnorderedParameterFilterResult> sut = Parameters.Any;

			Filter<Type> typeFilter = Have.Method
				.With(
					sut.OfType<Bar>())
				.ToTypeFilter();

			bool result = typeFilter.Applies(typeof(TestClass));
			result.Should().BeFalse();
		}

		[Fact]
		public void OfType_Unordered_OrOfType_NotMatchingAndMatchingType_ShouldReturnTrue()
		{
			IParameterFilter<IUnorderedParameterFilterResult> sut = Parameters.Any;

			Filter<Type> typeFilter = Have.Method
				.With(
					sut.OfType<Bar>().OrOfType<Foo>())
				.ToTypeFilter();

			bool result = typeFilter.Applies(typeof(TestClass));
			result.Should().BeTrue();
		}

		[Theory]
		[InlineData(typeof(Foo), true)]
		[InlineData(typeof(Bar), false)]
		public void OfType_Unordered_ShouldReturnExpectedValue(Type type, bool expectedValue)
		{
			IParameterFilter<IUnorderedParameterFilterResult> sut = Parameters.Any;

			Filter<Type> typeFilter = Have.Method
				.With(
					sut.OfType(type))
				.ToTypeFilter();

			bool result = typeFilter.Applies(typeof(TestClass));
			result.Should().Be(expectedValue);
		}

		private class TestClass
		{
			// ReSharper disable UnusedMember.Local
			// ReSharper disable UnusedParameter.Local
			public void MethodWithFooParameter(Foo value)
			{
				// Do nothing
			}
		}

		private class TestClassWithMultipleParameters
		{
			// ReSharper disable UnusedMember.Local
			// ReSharper disable UnusedParameter.Local
			public void MethodWithFooParameter(Foo value1, Bar value2)
			{
				// Do nothing
			}
		}

		public class Bar
		{
		}

		public class Foo : FooBase
		{
		}

		public class FooBase
		{
		}
	}
}
