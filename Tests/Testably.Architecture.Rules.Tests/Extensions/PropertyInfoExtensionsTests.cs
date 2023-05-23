using FluentAssertions;
using System;
using System.Reflection;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Extensions;

public sealed class PropertyInfoExtensionsTests
{
	[Fact]
	public void HasAttribute_WithAttribute_ShouldReturnTrue()
	{
		PropertyInfo type =
			typeof(TestClass).GetProperty(nameof(TestClass.Property1WithAttribute))!;

		bool result = type.HasAttribute<DummyAttribute>();

		result.Should().BeTrue();
	}

	[Fact]
	public void HasAttribute_WithInheritedAttribute_ShouldReturnTrue()
	{
		PropertyInfo type =
			typeof(TestClass).GetProperty(nameof(TestClass.PropertyWithAttributeInBaseClass))!;

		bool result = type.HasAttribute<DummyAttribute>();

		result.Should().BeTrue();
	}

	[Fact]
	public void HasAttribute_WithoutAttribute_ShouldReturnFalse()
	{
		PropertyInfo type =
			typeof(TestClass).GetProperty(nameof(TestClass.Property2WithoutAttribute))!;

		bool result = type.HasAttribute<DummyAttribute>();

		result.Should().BeFalse();
	}

	[Fact]
	public void HasAttribute_WithPredicate_ShouldReturnPredicateResult()
	{
		PropertyInfo type =
			typeof(TestClass).GetProperty(nameof(TestClass.Property1WithAttribute))!;

		bool result1 = type.HasAttribute<DummyAttribute>(d => d.Value == 1);
		bool result2 = type.HasAttribute<DummyAttribute>(d => d.Value == 2);

		result1.Should().BeTrue();
		result2.Should().BeFalse();
	}

	[AttributeUsage(AttributeTargets.Property)]
	private class DummyAttribute : Attribute
	{
		public int Value { get; }

		public DummyAttribute(int value)
		{
			Value = value;
		}
	}

	private class TestClass : TestClassBase
	{
		[Dummy(1)]
		public int Property1WithAttribute { get; set; }

		public int Property2WithoutAttribute { get; set; }

		public override int PropertyWithAttributeInBaseClass { get; set; }
	}

	private class TestClassBase
	{
		[Dummy(1)]
		public virtual int PropertyWithAttributeInBaseClass { get; set; }
	}
}
