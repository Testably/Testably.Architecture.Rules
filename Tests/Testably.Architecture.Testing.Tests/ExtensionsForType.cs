﻿using FluentAssertions;
using System;
using Xunit;

namespace Testably.Architecture.Testing.Tests;

public sealed class ExtensionsForType
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

	[Fact]
	public void HasMethodWithAttribute_WithAttribute_ShouldReturnFalse()
	{
		Type type = typeof(TestClassWithAttribute);

		bool result = type.HasMethodWithAttribute<DummyAttribute>();

		result.Should().BeFalse();
	}

	[Fact]
	public void HasMethodWithAttribute_WithoutAttribute_ShouldReturnTrue()
	{
		Type type = typeof(TestClassWithoutAttribute);

		bool result = type.HasMethodWithAttribute<DummyAttribute>();

		result.Should().BeTrue();
	}

	[Fact]
	public void HasMethodWithAttribute_WithPredicate_ShouldReturnPredicateResult()
	{
		Type type = typeof(TestClassWithoutAttribute);

		bool result1 = type.HasMethodWithAttribute<DummyAttribute>((d, _) => d.Value == 1);
		bool result2 = type.HasMethodWithAttribute<DummyAttribute>((d, _) => d.Value == 2);
		bool result3 = type.HasMethodWithAttribute<DummyAttribute>((d, _) => d.Value == 3);

		result1.Should().BeTrue();
		result2.Should().BeTrue();
		result3.Should().BeFalse();
	}

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
		{
			throw new NotImplementedException();
		}

		// ReSharper disable once UnusedMember.Local
		public void Method2WithoutAttribute()
		{
			throw new NotImplementedException();
		}
	}

	private class TestClassWithoutAttribute
	{
		[Dummy(1)]
		// ReSharper disable once UnusedMember.Local
		public void Method1WithAttribute()
		{
			throw new NotImplementedException();
		}

		[Dummy(2)]
		// ReSharper disable once UnusedMember.Local
		public void Method2WithAttribute()
		{
			throw new NotImplementedException();
		}
	}

	private class TestClassWithInheritedAttribute : TestClassWithAttribute
	{
	}
}
