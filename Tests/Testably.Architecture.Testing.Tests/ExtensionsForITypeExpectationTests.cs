using FluentAssertions;
using Xunit;

namespace Testably.Architecture.Testing.Tests;

public sealed class ExtensionsForITypeExpectationTests
{
	[Fact]
	public void ShouldBeSealed_SealedType_ShouldBeSatisfied()
	{
		IFilterableTypeExpectation sut = Expect.That
			.Type(typeof(SealedType));

		ITestResult<ITypeExpectation> result = sut.ShouldBeSealed();

		result.IsSatisfied.Should().BeTrue();
	}

	[Fact]
	public void ShouldBeSealed_UnsealedType_ShouldNotBeSatisfied()
	{
		IFilterableTypeExpectation sut = Expect.That
			.Type(typeof(UnsealedType));

		ITestResult<ITypeExpectation> result = sut.ShouldBeSealed();

		result.IsSatisfied.Should().BeFalse();
	}

	[Fact]
	public void ShouldBeSealed_WithParameterSetToFalse_ShouldReverseExpectedValue()
	{
		IFilterableTypeExpectation sut = Expect.That
			.Type(typeof(UnsealedType));

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
