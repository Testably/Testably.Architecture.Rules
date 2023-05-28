using FluentAssertions;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Filters;

// ReSharper disable UnusedMember.Local
public sealed partial class MethodFilterExtensionsTests
{
	public sealed class WithReturnTypeTests
	{
		[Fact]
		public void OrReturnType_ShouldUseCorrectErrorMessage()
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(BarClass))
				.Should(Have.Method.WithReturnType<DummyFooClass>())
				.Check.InAllLoadedAssemblies();

			result.Errors.Length.Should().Be(1);
			result.Errors[0].ToString().Should()
				.Contain(
					$"type '{typeof(BarClass)}' should have a method with return type {nameof(DummyFooClass)}");
		}

		[Fact]
		public void OrReturnType_WithGenericParameter_ShouldReturnBothTypes()
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(BarClass), typeof(FooClass))
				.Should(Have.Method.WithReturnType<DummyFooClass>().OrReturnType<DummyBarClass>())
				.Check.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
		}

		[Fact]
		public void OrReturnType_WithType_ShouldReturnBothTypes()
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(BarClass), typeof(FooClass))
				.Should(Have.Method.WithReturnType(typeof(DummyFooClass))
					.OrReturnType(typeof(DummyBarClass)))
				.Check.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
		}

		private class BarClass
		{
			public DummyBarClass BarMethod(int value)
				=> new(value);
		}

		private class FooClass
		{
			public DummyFooClass FooMethod(int value)
				=> new(value);
		}
	}
}
