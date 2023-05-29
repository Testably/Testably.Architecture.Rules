using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Extensions;

// ReSharper disable UnusedMember.Local
public sealed class TypeExtensionsTests
{
	[Fact]
	public void HasAttribute_WithAttribute_ShouldReturnTrue()
	{
		Type type = typeof(TestClassWithAttribute);

		bool result = type.HasAttribute<DummyAttribute>();

		result.Should().BeTrue();
	}

	[Fact]
	public void HasAttribute_WithInheritedAttribute_ShouldReturnTrue()
	{
		Type type = typeof(TestClassWithInheritedAttribute);

		bool result = type.HasAttribute<DummyAttribute>();

		result.Should().BeTrue();
	}

	[Fact]
	public void HasAttribute_WithoutAttribute_ShouldReturnFalse()
	{
		Type type = typeof(TestClassWithoutAttribute);

		bool result = type.HasAttribute<DummyAttribute>();

		result.Should().BeFalse();
	}

	[Fact]
	public void HasAttribute_WithPredicate_ShouldReturnPredicateResult()
	{
		Type type = typeof(TestClassWithAttribute);

		bool result1 = type.HasAttribute<DummyAttribute>(d => d.Value == 1);
		bool result2 = type.HasAttribute<DummyAttribute>(d => d.Value == 2);

		result1.Should().BeTrue();
		result2.Should().BeFalse();
	}

	[Theory]
	[InlineData(false)]
	[InlineData(true)]
	public void Implements_Object_ShouldReturnFalse(bool forceDirect)
	{
		Type sut = typeof(object);

		bool result = sut.Implements(typeof(IFooInterface), forceDirect);

		result.Should().BeFalse();
	}

	[Fact]
	public void InheritsFrom_SameType_ShouldReturnFalse()
	{
		Type sut = typeof(TestClassWithAttribute);

		bool result = sut.InheritsFrom(typeof(TestClassWithAttribute));

		result.Should().BeFalse();
	}

	[Fact]
	public void IsEqualTo_DifferentClosedGenericType_With2Parameters_ShouldReturnFalse()
	{
		Type sut = typeof(Dictionary<int, string>);

		bool result = sut.IsEqualTo(typeof(Dictionary<int, int>));

		result.Should().BeFalse();
	}

	[Fact]
	public void IsEqualTo_DifferentClosedGenericTypeWith1Parameter_ShouldReturnFalse()
	{
		Type sut = typeof(List<string>);

		bool result = sut.IsEqualTo(typeof(List<int>));

		result.Should().BeFalse();
	}

	[Fact]
	public void IsEqualTo_DifferentNumberOfClosedGenericParameters_ShouldReturnFalse()
	{
		Type sut = typeof(Tuple<int, string>);

		bool result = sut.IsEqualTo(typeof(Tuple<int, string, int>));

		result.Should().BeFalse();
	}

	[Fact]
	public void IsEqualTo_DifferentNumberOfOpenGenericParameters_ShouldReturnFalse()
	{
		Type sut = typeof(Tuple<,>);

		bool result = sut.IsEqualTo(typeof(Tuple<,,>));

		result.Should().BeFalse();
	}

	[Fact]
	public void IsEqualTo_SameClosedGenericTypeWith1Parameter_ShouldReturnTrue()
	{
		Type sut = typeof(List<string>);

		bool result = sut.IsEqualTo(typeof(List<string>));

		result.Should().BeTrue();
	}

	[Fact]
	public void IsEqualTo_SameClosedGenericTypeWith2Parameters_ShouldReturnTrue()
	{
		Type sut = typeof(Dictionary<int, string>);

		bool result = sut.IsEqualTo(typeof(Dictionary<int, string>));

		result.Should().BeTrue();
	}

	[Fact]
	public void IsEqualTo_SameGenericType_OtherOpen_ShouldReturnTrue()
	{
		Type sut = typeof(Task<>);

		bool result = sut.IsEqualTo(typeof(Task<int>));

		result.Should().BeTrue();
	}

	[Fact]
	public void IsEqualTo_SameGenericType_TypeOpen_ShouldReturnTrue()
	{
		Type sut = typeof(Dictionary<int, string>);

		bool result = sut.IsEqualTo(typeof(Dictionary<,>));

		result.Should().BeTrue();
	}

	[Fact]
	public void IsOrInheritsFrom_SameType_ShouldReturnTrue()
	{
		Type sut = typeof(TestClassWithAttribute);

		bool result = sut.IsOrInheritsFrom(typeof(TestClassWithAttribute));

		result.Should().BeTrue();
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

	private interface IFooInterface
	{
	}

	[Dummy(1)]
	private class TestClassWithAttribute
	{
		public void Method1WithoutAttribute()
			=> throw new NotSupportedException();

		public void Method2WithoutAttribute()
			=> throw new NotSupportedException();
	}

	private class TestClassWithInheritedAttribute : TestClassWithAttribute
	{
	}

	private class TestClassWithoutAttribute
	{
		[Dummy(1)]
		public void Method1WithAttribute()
			=> throw new NotSupportedException();

		[Dummy(2)]
		public void Method2WithAttribute()
			=> throw new NotSupportedException();
	}
}
