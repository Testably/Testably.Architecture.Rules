using FluentAssertions;
using System;
using System.Reflection;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Extensions;

public sealed class EventInfoExtensionsTests
{
	[Fact]
	public void HasAttribute_WithAttribute_ShouldReturnTrue()
	{
		EventInfo type = typeof(TestClass).GetEvent(nameof(TestClass.Event1WithAttribute))!;

		bool result = type.HasAttribute<DummyAttribute>();

		result.Should().BeTrue();
	}

	[Fact]
	public void HasAttribute_WithInheritedAttribute_ShouldReturnTrue()
	{
		EventInfo type =
			typeof(TestClass).GetEvent(nameof(TestClass.EventWithAttributeInBaseClass))!;

		bool result = type.HasAttribute<DummyAttribute>();

		result.Should().BeTrue();
	}

	[Fact]
	public void HasAttribute_WithoutAttribute_ShouldReturnFalse()
	{
		EventInfo type = typeof(TestClass).GetEvent(nameof(TestClass.Event2WithoutAttribute))!;

		bool result = type.HasAttribute<DummyAttribute>();

		result.Should().BeFalse();
	}

	[Fact]
	public void HasAttribute_WithPredicate_ShouldReturnPredicateResult()
	{
		EventInfo type = typeof(TestClass).GetEvent(nameof(TestClass.Event1WithAttribute))!;

		bool result1 = type.HasAttribute<DummyAttribute>(d => d.Value == 1);
		bool result2 = type.HasAttribute<DummyAttribute>(d => d.Value == 2);

		result1.Should().BeTrue();
		result2.Should().BeFalse();
	}

	[AttributeUsage(AttributeTargets.Event)]
	private class DummyAttribute : Attribute
	{
		public int Value { get; }

		public DummyAttribute(int value)
		{
			Value = value;
		}
	}
	#pragma warning disable CS8618
	public delegate void Dummy();

	private class TestClass : TestClassBase
	{
		[Dummy(1)]
		public event Dummy Event1WithAttribute;

		public event Dummy Event2WithoutAttribute;

		public override event Dummy EventWithAttributeInBaseClass;
	}

	private class TestClassBase
	{
		[Dummy(1)]
		public virtual event Dummy EventWithAttributeInBaseClass;
	}
	#pragma warning restore CS8618
}
