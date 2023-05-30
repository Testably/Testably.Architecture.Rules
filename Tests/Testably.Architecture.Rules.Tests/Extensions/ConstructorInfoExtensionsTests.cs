using FluentAssertions;
using System;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Extensions;

// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedMember.Local
public sealed class ConstructorInfoExtensionsTests
{
	[Fact]
	public void HasAttribute_WithAttribute_ShouldReturnTrue()
	{
		ConstructorInfo type = typeof(TestClass).GetDeclaredConstructors()
			.Single(c => c.GetParameters().Length == 2);

		bool result = type.HasAttribute<DummyAttribute>();

		result.Should().BeTrue();
	}

	[Fact]
	public void HasAttribute_WithoutAttribute_ShouldReturnFalse()
	{
		ConstructorInfo type = typeof(TestClass).GetDeclaredConstructors()
			.Single(c => c.GetParameters().Length == 3);

		bool result = type.HasAttribute<DummyAttribute>();

		result.Should().BeFalse();
	}

	[Fact]
	public void HasAttribute_WithPredicate_ShouldReturnPredicateResult()
	{
		ConstructorInfo type = typeof(TestClass).GetDeclaredConstructors()
			.Single(c => c.GetParameters().Length == 2);

		bool result1 = type.HasAttribute<DummyAttribute>(d => d.Value == 1);
		bool result2 = type.HasAttribute<DummyAttribute>(d => d.Value == 2);

		result1.Should().BeTrue();
		result2.Should().BeFalse();
	}

	[AttributeUsage(AttributeTargets.Constructor)]
	private class DummyAttribute : Attribute
	{
		public int Value { get; }

		public DummyAttribute(int value)
		{
			Value = value;
		}
	}

	private class TestClass
	{
		[Dummy(1)]
		public TestClass(int value1, int value2)
		{
		}

		public TestClass(int value1, int value2, int value3)
		{
		}
	}
}
