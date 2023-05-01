using FluentAssertions;
using System;
using Testably.Architecture.Testing.TestErrors;
using Xunit;

namespace Testably.Architecture.Testing.Tests;

public sealed partial class ExtensionsForITypeExpectationTests
{
	public sealed class BeGeneric
	{
		[Fact]
		public void ShouldBeGeneric_GenericType_ShouldBeSatisfied()
		{
			Type type = typeof(GenericType<>);
			IFilterableTypeExpectation sut = Expect.That.Type(type);

			ITestResult<ITypeExpectation> result = sut.ShouldBeGeneric();

			result.IsSatisfied.Should().BeTrue();
		}

		[Fact]
		public void ShouldBeGeneric_SpecificType_ShouldNotBeSatisfied()
		{
			Type type = typeof(SpecificType);
			IFilterableTypeExpectation sut = Expect.That.Type(type);

			ITestResult<ITypeExpectation> result = sut.ShouldBeGeneric();

			result.IsSatisfied.Should().BeFalse();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
			result.Errors[0].ToString().Should().Contain("should be generic");
		}

		[Fact]
		public void ShouldNotBeGeneric_GenericType_ShouldNotBeSatisfied()
		{
			Type type = typeof(GenericType<>);
			IFilterableTypeExpectation sut = Expect.That.Type(type);

			ITestResult<ITypeExpectation> result = sut.ShouldNotBeGeneric();

			result.IsSatisfied.Should().BeFalse();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
			result.Errors[0].ToString().Should().Contain("should not be generic");
		}

		[Fact]
		public void ShouldNotBeGeneric_SpecificType_ShouldBeSatisfied()
		{
			Type type = typeof(SpecificType);
			IFilterableTypeExpectation sut = Expect.That.Type(type);

			ITestResult<ITypeExpectation> result = sut.ShouldNotBeGeneric();

			result.IsSatisfied.Should().BeTrue();
		}

		// ReSharper disable once UnusedTypeParameter
		private class GenericType<T>
		{
		}

		private class SpecificType
		{
		}
	}
}
