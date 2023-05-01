using FluentAssertions;
using System;
using Testably.Architecture.Testing.TestErrors;
using Xunit;

namespace Testably.Architecture.Testing.Tests;

public sealed partial class ExtensionsForITypeExpectationTests
{
	public sealed class ShouldBeSealed
	{
		[Fact]
		public void ShouldBeSealed_SealedType_ShouldBeSatisfied()
		{
			Type type = typeof(SealedType);
			IFilterableTypeExpectation sut = Expect.That.Type(type);

			ITestResult<ITypeExpectation> result = sut.ShouldBeSealed();

			result.IsSatisfied.Should().BeTrue();
		}

		[Fact]
		public void ShouldBeSealed_UnsealedType_ShouldNotBeSatisfied()
		{
			Type type = typeof(UnsealedType);
			IFilterableTypeExpectation sut = Expect.That.Type(type);

			ITestResult<ITypeExpectation> result = sut.ShouldBeSealed();

			result.IsSatisfied.Should().BeFalse();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
		}

		[Fact]
		public void ShouldBeSealed_WithParameterSetToFalse_ShouldReverseExpectedValue()
		{
			Type type = typeof(UnsealedType);
			IFilterableTypeExpectation sut = Expect.That.Type(type);

			ITestResult<ITypeExpectation> result = sut.ShouldBeSealed(false);

			result.IsSatisfied.Should().BeTrue();
		}

		private sealed class SealedType
		{
		}

		private class UnsealedType
		{
		}
	}
}
