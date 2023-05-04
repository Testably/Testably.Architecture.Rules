using FluentAssertions;
using System;
using Xunit;

namespace Testably.Architecture.Testing.Tests.Requirements;

public sealed partial class RequirementOnTypeExtensionsTests
{
	public sealed class BeAClass
	{
		[Fact]
		public void ShouldBeAClass_ClassType_ShouldBeSatisfied()
		{
			Type type = typeof(ClassType);
			ITypeExpectation sut = Expect.That.Type(type);

			IRequirementResult<Type> result = sut.ShouldBeAClass();

			result.IsSatisfied.Should().BeTrue();
		}

		[Fact]
		public void ShouldBeAClass_EnumType_ShouldNotBeSatisfied()
		{
			Type type = typeof(EnumType);
			ITypeExpectation sut = Expect.That.Type(type);

			IRequirementResult<Type> result = sut.ShouldBeAClass();

			result.IsSatisfied.Should().BeFalse();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
			result.Errors[0].ToString().Should().Contain("should be a class");
		}

		[Fact]
		public void ShouldNotBeAClass_ClassType_ShouldNotBeSatisfied()
		{
			Type type = typeof(ClassType);
			ITypeExpectation sut = Expect.That.Type(type);

			IRequirementResult<Type> result = sut.ShouldNotBeAClass();

			result.IsSatisfied.Should().BeFalse();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
			result.Errors[0].ToString().Should().Contain("should not be a class");
		}

		[Fact]
		public void ShouldNotBeAClass_EnumType_ShouldBeSatisfied()
		{
			Type type = typeof(EnumType);
			ITypeExpectation sut = Expect.That.Type(type);

			IRequirementResult<Type> result = sut.ShouldNotBeAClass();

			result.IsSatisfied.Should().BeTrue();
		}

		private class ClassType
		{
		}

		private enum EnumType
		{
		}
	}
}
