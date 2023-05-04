using FluentAssertions;
using System;
using Xunit;

namespace Testably.Architecture.Testing.Tests.Requirements;

public sealed partial class RequirementOnTypeExtensionsTests
{
	public sealed class BeNested
	{
		[Fact]
		public void ShouldBeNested_NestedType_ShouldBeSatisfied()
		{
			Type type = typeof(NestedType);
			ITypeExpectation sut = Expect.That.Type(type);

			IRequirementResult<Type> result = sut.ShouldBeNested();

			result.IsSatisfied.Should().BeTrue();
		}

		[Fact]
		public void ShouldBeNested_UnnestedType_ShouldNotBeSatisfied()
		{
			Type type = typeof(RequirementOnTypeExtensionsTests);
			ITypeExpectation sut = Expect.That.Type(type);

			IRequirementResult<Type> result = sut.ShouldBeNested();

			result.IsSatisfied.Should().BeFalse();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
			result.Errors[0].ToString().Should().Contain("should be nested");
		}

		[Fact]
		public void ShouldNotBeNested_NestedType_ShouldNotBeSatisfied()
		{
			Type type = typeof(NestedType);
			ITypeExpectation sut = Expect.That.Type(type);

			IRequirementResult<Type> result = sut.ShouldNotBeNested();

			result.IsSatisfied.Should().BeFalse();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
			result.Errors[0].ToString().Should().Contain("should not be nested");
		}

		[Fact]
		public void ShouldNotBeNested_UnnestedType_ShouldBeSatisfied()
		{
			Type type = typeof(RequirementOnTypeExtensionsTests);
			ITypeExpectation sut = Expect.That.Type(type);

			IRequirementResult<Type> result = sut.ShouldNotBeNested();

			result.IsSatisfied.Should().BeTrue();
		}

		public class NestedType
		{
		}
	}
}
