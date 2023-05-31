using FluentAssertions;
using System;
using System.Linq;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Filters;

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local
public sealed partial class ParameterFilterExtensionsTests
{
	public sealed class OfTypeTests
	{
		[Theory]
		[InlineData(typeof(Foo), typeof(Bar), true)]
		[InlineData(typeof(Bar), typeof(Foo), false)]
		public void OfType_Ordered_At_ShouldGoToNextParameter(Type type1, Type type2,
			bool expectedValue)
		{
			IParameterFilter<IOrderedParameterFilterResult> sut = Parameters.At(1);

			bool result = Have.Method
				.With(sut
					.OfType(type2)
					.At(0)
					.OfType(type1))
				.Applies(typeof(TestClassWithMultipleParameters).GetDeclaredMethods().First());

			result.Should().Be(expectedValue);
		}

		[Theory]
		[InlineData(true, true)]
		[InlineData(false, false)]
		public void OfType_Ordered_FooBase_ShouldReturnExpectedValue(bool allowDerivedType,
			bool expectedValue)
		{
			IParameterFilter<IOrderedParameterFilterResult> sut = Parameters.First;

			bool result = Have.Method
				.With(
					sut.OfType<FooBase>(allowDerivedType))
				.Applies(typeof(TestClass).GetDeclaredMethods().First());

			result.Should().Be(expectedValue);
		}

		[Fact]
		public void OfType_Ordered_NotMatchingType_ShouldReturnFalse()
		{
			IParameterFilter<IOrderedParameterFilterResult> sut = Parameters.First;

			bool result = Have.Method
				.With(
					sut.OfType<Bar>())
				.Applies(typeof(TestClass).GetDeclaredMethods().First());

			result.Should().BeFalse();
		}

		[Fact]
		public void OfType_Ordered_OrOfType_NotMatchingAndMatchingType_ShouldReturnTrue()
		{
			IParameterFilter<IOrderedParameterFilterResult> sut = Parameters.First;

			bool result = Have.Method
				.With(
					sut.OfType<Bar>().OrOfType<Foo>())
				.Applies(typeof(TestClass).GetDeclaredMethods().First());

			result.Should().BeTrue();
		}

		[Fact]
		public void OfType_Ordered_OrOfType_ToString_ShouldCombineBothTypes()
		{
			IParameterFilter<IOrderedParameterFilterResult> sut = Parameters.First;

			string result = sut.OfType<Foo>().OrOfType<Bar>().ToString();

			result.Should().Contain($"(is of type {nameof(Foo)} or is of type {nameof(Bar)})");
		}

		[Theory]
		[InlineData(typeof(Foo), true)]
		[InlineData(typeof(Bar), false)]
		public void OfType_Ordered_ShouldReturnExpectedValue(Type type, bool expectedValue)
		{
			IParameterFilter<IOrderedParameterFilterResult> sut = Parameters.First;

			bool result = Have.Method
				.With(
					sut.OfType(type))
				.Applies(typeof(TestClass).GetDeclaredMethods().First());

			result.Should().Be(expectedValue);
		}

		[Theory]
		[InlineData(typeof(Foo), typeof(Bar), true)]
		[InlineData(typeof(Bar), typeof(Foo), false)]
		public void OfType_Ordered_Then_ShouldGoToNextParameter(Type type1, Type type2,
			bool expectedValue)
		{
			IParameterFilter<IOrderedParameterFilterResult> sut = Parameters.First;

			bool result = Have.Method
				.With(sut
					.OfType(type1)
					.Then()
					.OfType(type2))
				.Applies(typeof(TestClassWithMultipleParameters).GetDeclaredMethods().First());

			result.Should().Be(expectedValue);
		}

		[Fact]
		public void OfType_Ordered_ToString_ShouldCombineBothTypes()
		{
			IParameterFilter<IOrderedParameterFilterResult> sut = Parameters.First;

			string result = sut.OfType<Foo>().ToString();

			result.Should().Be($"is of type {nameof(Foo)}");
		}

		[Theory]
		[InlineData(true, true)]
		[InlineData(false, false)]
		public void OfType_Unordered_FooBase_ShouldReturnExpectedValue(bool allowDerivedType,
			bool expectedValue)
		{
			IParameterFilter<IUnorderedParameterFilterResult> sut = Parameters.Any;

			bool result = Have.Method
				.With(
					sut.OfType<FooBase>(allowDerivedType))
				.Applies(typeof(TestClass).GetDeclaredMethods().First());

			result.Should().Be(expectedValue);
		}

		[Fact]
		public void OfType_Unordered_NotMatchingType_ShouldReturnFalse()
		{
			IParameterFilter<IUnorderedParameterFilterResult> sut = Parameters.Any;

			bool result = Have.Method
				.With(
					sut.OfType<Bar>())
				.Applies(typeof(TestClass).GetDeclaredMethods().First());

			result.Should().BeFalse();
		}

		[Fact]
		public void OfType_Unordered_OrOfType_NotMatchingAndMatchingType_ShouldReturnTrue()
		{
			IParameterFilter<IUnorderedParameterFilterResult> sut = Parameters.Any;

			bool result = Have.Method
				.With(
					sut.OfType<Bar>().OrOfType<Foo>())
				.Applies(typeof(TestClass).GetDeclaredMethods().First());

			result.Should().BeTrue();
		}

		[Fact]
		public void OfType_Unordered_OrOfType_ToString_ShouldCombineBothTypes()
		{
			IParameterFilter<IUnorderedParameterFilterResult> sut = Parameters.Any;

			string result = sut.OfType<Foo>().OrOfType<Bar>().ToString();

			result.Should().Contain($"(is of type {nameof(Foo)} or is of type {nameof(Bar)})");
		}

		[Theory]
		[InlineData(typeof(Foo), true)]
		[InlineData(typeof(Bar), false)]
		public void OfType_Unordered_ShouldReturnExpectedValue(Type type, bool expectedValue)
		{
			IParameterFilter<IUnorderedParameterFilterResult> sut = Parameters.Any;

			bool result = Have.Method
				.With(
					sut.OfType(type))
				.Applies(typeof(TestClass).GetDeclaredMethods().First());

			result.Should().Be(expectedValue);
		}

		[Fact]
		public void OfType_Unordered_ToString_ShouldCombineBothTypes()
		{
			IParameterFilter<IUnorderedParameterFilterResult> sut = Parameters.Any;

			string result = sut.OfType<Foo>().ToString();

			result.Should().Be($"is of type {nameof(Foo)}");
		}

		#pragma warning disable CA1822
		private class TestClass
		{
			public void MethodWithFooParameter(Foo value)
			{
				// Do nothing
			}
		}

		private class TestClassWithMultipleParameters
		{
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
		#pragma warning restore CA1822
	}
}
