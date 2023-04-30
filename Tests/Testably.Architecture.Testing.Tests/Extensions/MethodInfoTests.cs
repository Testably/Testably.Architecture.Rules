using FluentAssertions;
using System;
using System.Reflection;
using Xunit;

namespace Testably.Architecture.Testing.Tests.Extensions;

public sealed class MethodInfoTests
{
	[Fact]
	public void HasAttribute_WithAttribute_ShouldReturnTrue()
	{
		MethodInfo type = typeof(TestClass).GetMethod(nameof(TestClass.Method1))!;

		bool result = type.HasAttribute<DummyAttribute>();

		result.Should().BeTrue();
	}

	[Fact]
	public void HasAttribute_WithPredicate_ShouldReturnPredicateResult()
	{
		MethodInfo type = typeof(TestClass).GetMethod(nameof(TestClass.Method1))!;

		bool result1 = type.HasAttribute<DummyAttribute>(d => d.Value == 1);
		bool result2 = type.HasAttribute<DummyAttribute>(d => d.Value == 2);

		result1.Should().BeTrue();
		result2.Should().BeFalse();
	}

	[Fact]
	public void HasAttribute_WithoutAttribute_ShouldReturnFalse()
	{
		MethodInfo type = typeof(TestClass).GetMethod(nameof(TestClass.Method2))!;

		bool result = type.HasAttribute<DummyAttribute>();

		result.Should().BeFalse();
	}

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
		// ReSharper disable once UnusedMember.Local
		public void Method1()
		{
			throw new NotImplementedException();
		}

		// ReSharper disable once UnusedMember.Local
		public void Method2()
		{
			throw new NotImplementedException();
		}
	}
}
