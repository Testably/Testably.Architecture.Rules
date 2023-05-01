using FluentAssertions;
using System;
using System.Reflection;
using Xunit;

namespace Testably.Architecture.Testing.Tests;

public sealed class ExtensionsForMethodInfoTests
{
	[Fact]
	public void HasAttribute_WithAttribute_ShouldReturnTrue()
	{
		MethodInfo type = typeof(TestClass).GetMethod(nameof(TestClass.Method1WithAttribute))!;

		bool result = type.HasAttribute<DummyAttribute>();

		result.Should().BeTrue();
	}

	[Fact]
	public void HasAttribute_WithInheritedAttribute_ShouldReturnTrue()
	{
		MethodInfo type =
			typeof(TestClass).GetMethod(nameof(TestClass.MethodWithAttributeInBaseClass))!;

		bool result = type.HasAttribute<DummyAttribute>();

		result.Should().BeTrue();
	}

	[Fact]
	public void HasAttribute_WithoutAttribute_ShouldReturnFalse()
	{
		MethodInfo type = typeof(TestClass).GetMethod(nameof(TestClass.Method2WithoutAttribute))!;

		bool result = type.HasAttribute<DummyAttribute>();

		result.Should().BeFalse();
	}

	[Fact]
	public void HasAttribute_WithPredicate_ShouldReturnPredicateResult()
	{
		MethodInfo type = typeof(TestClass).GetMethod(nameof(TestClass.Method1WithAttribute))!;

		bool result1 = type.HasAttribute<DummyAttribute>(d => d.Value == 1);
		bool result2 = type.HasAttribute<DummyAttribute>(d => d.Value == 2);

		result1.Should().BeTrue();
		result2.Should().BeFalse();
	}

	[AttributeUsage(AttributeTargets.Method)]
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
		// ReSharper disable once UnusedMember.Local
		public void Method1WithAttribute()
		{
			throw new NotImplementedException();
		}

		// ReSharper disable once UnusedMember.Local
		public void Method2WithoutAttribute()
		{
			throw new NotImplementedException();
		}

		// ReSharper disable once UnusedMember.Local
		public override void MethodWithAttributeInBaseClass()
		{
			throw new NotImplementedException();
		}
	}

	private class TestClassBase
	{
		[Dummy(1)]
		// ReSharper disable once UnusedMember.Local
		public virtual void MethodWithAttributeInBaseClass()
		{
			throw new NotImplementedException();
		}
	}
}
