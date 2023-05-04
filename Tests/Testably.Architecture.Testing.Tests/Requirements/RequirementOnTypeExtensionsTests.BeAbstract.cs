using FluentAssertions;
using System;
using Xunit;

namespace Testably.Architecture.Testing.Tests.Requirements;

public sealed partial class RequirementOnTypeExtensionsTests
{
	public sealed class BeAbstract
	{
		[Fact]
		public void ShouldBeAbstract_AbstractType_ShouldBeSatisfied()
		{
			Type type = typeof(AbstractType);
			ITypeExpectation sut = Expect.That.Type(type);

			IRequirementResult<Type> result = sut.ShouldBeAbstract();

			result.IsSatisfied.Should().BeTrue();
		}

		[Fact]
		public void ShouldBeAbstract_ConcreteType_ShouldNotBeSatisfied()
		{
			Type type = typeof(ConcreteType);
			ITypeExpectation sut = Expect.That.Type(type);

			IRequirementResult<Type> result = sut.ShouldBeAbstract();

			result.IsSatisfied.Should().BeFalse();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
			result.Errors[0].ToString().Should().Contain("should be abstract");
		}

		[Fact]
		public void ShouldNotBeAbstract_AbstractType_ShouldNotBeSatisfied()
		{
			Type type = typeof(AbstractType);
			ITypeExpectation sut = Expect.That.Type(type);

			IRequirementResult<Type> result = sut.ShouldNotBeAbstract();

			result.IsSatisfied.Should().BeFalse();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
			result.Errors[0].ToString().Should().Contain("should not be abstract");
		}

		[Fact]
		public void ShouldNotBeAbstract_ConcreteType_ShouldBeSatisfied()
		{
			Type type = typeof(ConcreteType);
			ITypeExpectation sut = Expect.That.Type(type);

			IRequirementResult<Type> result = sut.ShouldNotBeAbstract();

			result.IsSatisfied.Should().BeTrue();
		}

		private abstract class AbstractType
		{
		}

		private class ConcreteType
		{
		}
	}
}
