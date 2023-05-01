using FluentAssertions;
using System;
using Testably.Architecture.Testing.TestErrors;
using Xunit;

namespace Testably.Architecture.Testing.Tests;

public sealed partial class ExtensionsForITypeExpectationTests
{
	public sealed class BeAnInterface
	{
		[Fact]
		public void ShouldBeAnInterface_EnumType_ShouldNotBeSatisfied()
		{
			Type type = typeof(EnumType);
			IFilterableTypeExpectation sut = Expect.That.Type(type);

			ITestResult<ITypeExpectation> result = sut.ShouldBeAnInterface();

			result.IsSatisfied.Should().BeFalse();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
			result.Errors[0].ToString().Should().Contain("should be an interface");
		}

		[Fact]
		public void ShouldBeAnInterface_InterfaceType_ShouldBeSatisfied()
		{
			Type type = typeof(InterfaceType);
			IFilterableTypeExpectation sut = Expect.That.Type(type);

			ITestResult<ITypeExpectation> result = sut.ShouldBeAnInterface();

			result.IsSatisfied.Should().BeTrue();
		}

		[Fact]
		public void ShouldNotBeAnInterface_EnumType_ShouldBeSatisfied()
		{
			Type type = typeof(EnumType);
			IFilterableTypeExpectation sut = Expect.That.Type(type);

			ITestResult<ITypeExpectation> result = sut.ShouldNotBeAnInterface();

			result.IsSatisfied.Should().BeTrue();
		}

		[Fact]
		public void ShouldNotBeAnInterface_InterfaceType_ShouldNotBeSatisfied()
		{
			Type type = typeof(InterfaceType);
			IFilterableTypeExpectation sut = Expect.That.Type(type);

			ITestResult<ITypeExpectation> result = sut.ShouldNotBeAnInterface();

			result.IsSatisfied.Should().BeFalse();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
			result.Errors[0].ToString().Should().Contain("should not be an interface");
		}

		private enum EnumType
		{
		}

		private interface InterfaceType
		{
		}
	}
}
