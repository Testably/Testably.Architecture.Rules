using FluentAssertions;
using System;
using Testably.Architecture.Testing.TestErrors;
using Xunit;

namespace Testably.Architecture.Testing.Tests;

public sealed partial class ExtensionsForITypeExpectationTests
{
	public sealed class HaveAttribute
	{
		[Theory]
		[InlineData(false)]
		[InlineData(true)]
		public void ShouldHaveAttribute_Inherit_ShouldConsiderParameter(bool inherit)
		{
			Type type = typeof(TestClassWithInheritedAttribute);
			var sut = Expect.That.Type(type);

			IExpectationResult<Type> result = sut.ShouldHaveAttribute<DummyAttribute>(
				inherit: inherit);

			result.IsSatisfied.Should().Be(inherit);
		}

		[Theory]
		[InlineData(1, true)]
		[InlineData(2, false)]
		public void ShouldHaveAttribute_Predicate_ShouldConsiderParameter(int value,
			bool expectIsSatisfied)
		{
			Type type = typeof(TestClassWithInheritedAttribute);
			var sut = Expect.That.Type(type);

			IExpectationResult<Type> result = sut.ShouldHaveAttribute<DummyAttribute>(
				predicate: d => d.Value == value);

			result.IsSatisfied.Should().Be(expectIsSatisfied);
		}

		[Fact]
		public void ShouldHaveAttribute_WithAttribute_ShouldBeSatisfied()
		{
			Type type = typeof(TestClassWithAttribute);
			var sut = Expect.That.Type(type);

			IExpectationResult<Type> result = sut.ShouldHaveAttribute<DummyAttribute>();

			result.IsSatisfied.Should().BeTrue();
		}

		[Fact]
		public void ShouldHaveAttribute_WithoutAttribute_ShouldNotBeSatisfied()
		{
			Type type = typeof(TestClassWithoutAttribute);
			var sut = Expect.That.Type(type);

			IExpectationResult<Type> result = sut.ShouldHaveAttribute<DummyAttribute>();

			result.IsSatisfied.Should().BeFalse();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
		}

		[Theory]
		[InlineData(false)]
		[InlineData(true)]
		public void ShouldNotHaveAttribute_Inherit_ShouldConsiderParameter(bool inherit)
		{
			Type type = typeof(TestClassWithInheritedAttribute);
			var sut = Expect.That.Type(type);

			IExpectationResult<Type> result = sut.ShouldNotHaveAttribute<DummyAttribute>(
				inherit: inherit);

			result.IsSatisfied.Should().Be(!inherit);
		}

		[Theory]
		[InlineData(1, false)]
		[InlineData(2, true)]
		public void ShouldNotHaveAttribute_Predicate_ShouldConsiderParameter(
			int value,
			bool expectIsSatisfied)
		{
			Type type = typeof(TestClassWithInheritedAttribute);
			var sut = Expect.That.Type(type);

			IExpectationResult<Type> result = sut.ShouldNotHaveAttribute<DummyAttribute>(
				predicate: d => d.Value == value);

			result.IsSatisfied.Should().Be(expectIsSatisfied);
		}

		[Fact]
		public void ShouldNotHaveAttribute_WithAttribute_ShouldNotBeSatisfied()
		{
			Type type = typeof(TestClassWithAttribute);
			var sut = Expect.That.Type(type);

			IExpectationResult<Type> result = sut.ShouldNotHaveAttribute<DummyAttribute>();

			result.IsSatisfied.Should().BeFalse();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
		}

		[Fact]
		public void ShouldNotHaveAttribute_WithoutAttribute_ShouldBeSatisfied()
		{
			Type type = typeof(TestClassWithoutAttribute);
			var sut = Expect.That.Type(type);

			IExpectationResult<Type> result = sut.ShouldNotHaveAttribute<DummyAttribute>();

			result.IsSatisfied.Should().BeTrue();
		}

		[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
		private class DummyAttribute : Attribute
		{
			public int Value { get; }

			public DummyAttribute(int value)
			{
				Value = value;
			}
		}

		[Dummy(1)]
		private class TestClassWithAttribute
		{
			// ReSharper disable once UnusedMember.Local
			public void Method1WithoutAttribute()
				=> throw new NotSupportedException();

			// ReSharper disable once UnusedMember.Local
			public void Method2WithoutAttribute()
				=> throw new NotSupportedException();
		}

		private class TestClassWithInheritedAttribute : TestClassWithAttribute
		{
		}

		private class TestClassWithoutAttribute
		{
			[Dummy(1)]
			// ReSharper disable once UnusedMember.Local
			public void Method1WithAttribute()
				=> throw new NotSupportedException();

			[Dummy(2)]
			// ReSharper disable once UnusedMember.Local
			public void Method2WithAttribute()
				=> throw new NotSupportedException();
		}
	}
}
