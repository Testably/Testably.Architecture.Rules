using FluentAssertions;
using System;
using Xunit;

namespace Testably.Architecture.Testing.Tests.Requirements;

public sealed partial class RequirementOnTypeExtensionsTests
{
	public sealed class BeGeneric
	{
		[Fact]
		public void ShouldBeGeneric_GenericType_ShouldBeSatisfied()
		{
			Type type = typeof(GenericType<>);
			ITypeExpectation sut = Expect.That.Type(type);

			IRequirementResult<Type> result = sut.ShouldBeGeneric();

			result.IsSatisfied.Should().BeTrue();
		}

		[Fact]
		public void ShouldBeGeneric_SpecificType_ShouldNotBeSatisfied()
		{
			Type type = typeof(SpecificType);
			ITypeExpectation sut = Expect.That.Type(type);

			IRequirementResult<Type> result = sut.ShouldBeGeneric();

			result.IsSatisfied.Should().BeFalse();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
			result.Errors[0].ToString().Should().Contain("should be generic");
		}

		[Fact]
		public void ShouldNotBeGeneric_GenericType_ShouldNotBeSatisfied()
		{
			Type type = typeof(GenericType<>);
			ITypeExpectation sut = Expect.That.Type(type);

			IRequirementResult<Type> result = sut.ShouldNotBeGeneric();

			result.IsSatisfied.Should().BeFalse();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
			result.Errors[0].ToString().Should().Contain("should not be generic");
		}

		[Fact]
		public void ShouldNotBeGeneric_SpecificType_ShouldBeSatisfied()
		{
			Type type = typeof(SpecificType);
			ITypeExpectation sut = Expect.That.Type(type);

			IRequirementResult<Type> result = sut.ShouldNotBeGeneric();

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
