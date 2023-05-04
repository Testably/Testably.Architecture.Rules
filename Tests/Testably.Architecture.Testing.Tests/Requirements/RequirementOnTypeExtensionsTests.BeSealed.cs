using FluentAssertions;
using System;
using Xunit;

namespace Testably.Architecture.Testing.Tests.Requirements;

public sealed partial class RequirementOnTypeExtensionsTests
{
	public sealed class BeSealed
	{
		[Fact]
		public void ShouldBeSealed_SealedType_ShouldBeSatisfied()
		{
			Type type = typeof(SealedType);
			ITypeExpectation sut = Expect.That.Type(type);

			IRequirementResult<Type> result = sut.ShouldBeSealed();

			result.IsSatisfied.Should().BeTrue();
		}

		[Fact]
		public void ShouldBeSealed_UnsealedType_ShouldNotBeSatisfied()
		{
			Type type = typeof(UnsealedType);
			ITypeExpectation sut = Expect.That.Type(type);

			IRequirementResult<Type> result = sut.ShouldBeSealed();

			result.IsSatisfied.Should().BeFalse();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
			result.Errors[0].ToString().Should().Contain("should be sealed");
		}

		[Fact]
		public void ShouldNotBeSealed_SealedType_ShouldNotBeSatisfied()
		{
			Type type = typeof(SealedType);
			ITypeExpectation sut = Expect.That.Type(type);

			IRequirementResult<Type> result = sut.ShouldNotBeSealed();

			result.IsSatisfied.Should().BeFalse();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
			result.Errors[0].ToString().Should().Contain("should not be sealed");
		}

		[Fact]
		public void ShouldNotBeSealed_UnsealedType_ShouldBeSatisfied()
		{
			Type type = typeof(UnsealedType);
			ITypeExpectation sut = Expect.That.Type(type);

			IRequirementResult<Type> result = sut.ShouldNotBeSealed();

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
