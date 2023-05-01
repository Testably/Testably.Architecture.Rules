using FluentAssertions;
using System;
using Testably.Architecture.Testing.TestErrors;
using Xunit;

namespace Testably.Architecture.Testing.Tests;

public sealed partial class ExtensionsForITypeExpectationTests
{
	public sealed class ShouldHaveAttribute
	{
		[Theory]
		[InlineData(false)]
		[InlineData(true)]
		public void ShouldHaveAttribute_Inherit_ShouldConsiderParameter(bool inherit)
		{
			Type type = typeof(TestClassWithInheritedAttribute);
			IFilterableTypeExpectation sut = Expect.That.Type(type);

			ITestResult<ITypeExpectation> result = sut.ShouldHaveAttribute<DummyAttribute>(
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
			IFilterableTypeExpectation sut = Expect.That.Type(type);

			ITestResult<ITypeExpectation> result = sut.ShouldHaveAttribute<DummyAttribute>(
				predicate: d => d.Value == value);

			result.IsSatisfied.Should().Be(expectIsSatisfied);
		}

		[Fact]
		public void ShouldHaveAttribute_WithAttribute_ShouldBeSatisfied()
		{
			Type type = typeof(TestClassWithAttribute);
			IFilterableTypeExpectation sut = Expect.That.Type(type);

			ITestResult<ITypeExpectation> result = sut.ShouldHaveAttribute<DummyAttribute>();

			result.IsSatisfied.Should().BeTrue();
		}

		[Fact]
		public void ShouldHaveAttribute_WithoutAttribute_ShouldNotBeSatisfied()
		{
			Type type = typeof(TestClassWithoutAttribute);
			IFilterableTypeExpectation sut = Expect.That.Type(type);

			ITestResult<ITypeExpectation> result = sut.ShouldHaveAttribute<DummyAttribute>();

			result.IsSatisfied.Should().BeFalse();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
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
