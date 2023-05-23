using FluentAssertions;
using System;
using System.Reflection;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Extensions;

public sealed class FieldInfoExtensionsTests
{
	[Fact]
	public void HasAttribute_WithAttribute_ShouldReturnTrue()
	{
		FieldInfo type = typeof(TestClass).GetField(nameof(TestClass.Field1WithAttribute))!;

		bool result = type.HasAttribute<DummyAttribute>();

		result.Should().BeTrue();
	}

	[Fact]
	public void HasAttribute_WithoutAttribute_ShouldReturnFalse()
	{
		FieldInfo type = typeof(TestClass).GetField(nameof(TestClass.Field2WithoutAttribute))!;

		bool result = type.HasAttribute<DummyAttribute>();

		result.Should().BeFalse();
	}

	[Fact]
	public void HasAttribute_WithPredicate_ShouldReturnPredicateResult()
	{
		FieldInfo type = typeof(TestClass).GetField(nameof(TestClass.Field1WithAttribute))!;

		bool result1 = type.HasAttribute<DummyAttribute>(d => d.Value == 1);
		bool result2 = type.HasAttribute<DummyAttribute>(d => d.Value == 2);

		result1.Should().BeTrue();
		result2.Should().BeFalse();
	}

	[AttributeUsage(AttributeTargets.Field)]
	private class DummyAttribute : Attribute
	{
		public int Value { get; }

		public DummyAttribute(int value)
		{
			Value = value;
		}
	}

	#pragma warning disable CS0649
	private class TestClass
	{
		[Dummy(1)]
		public int Field1WithAttribute;
		
		public int Field2WithoutAttribute;
	}
	#pragma warning restore CS0649
}
