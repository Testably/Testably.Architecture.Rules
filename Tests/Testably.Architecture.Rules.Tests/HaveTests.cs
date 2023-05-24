using FluentAssertions;
using Xunit;

namespace Testably.Architecture.Rules.Tests;

public sealed class HaveTests
{
	[Fact]
	public void Constructor_ShouldReturnDifferentInstances()
	{
		IConstructorFilter result1 = Have.Constructor;
		IConstructorFilter result2 = Have.Constructor;

		result1.Should().NotBe(result2);
	}

	[Fact]
	public void Event_ShouldReturnDifferentInstances()
	{
		IEventFilter result1 = Have.Event;
		IEventFilter result2 = Have.Event;

		result1.Should().NotBe(result2);
	}

	[Fact]
	public void Field_ShouldReturnDifferentInstances()
	{
		IFieldFilter result1 = Have.Field;
		IFieldFilter result2 = Have.Field;

		result1.Should().NotBe(result2);
	}

	[Fact]
	public void Method_ShouldReturnDifferentInstances()
	{
		IMethodFilter result1 = Have.Method;
		IMethodFilter result2 = Have.Method;

		result1.Should().NotBe(result2);
	}

	[Fact]
	public void Property_ShouldReturnDifferentInstances()
	{
		IPropertyFilter result1 = Have.Property;
		IPropertyFilter result2 = Have.Property;

		result1.Should().NotBe(result2);
	}
}
