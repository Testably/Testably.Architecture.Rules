using FluentAssertions;
using System;
using Testably.Architecture.Testing.TestErrors;
using Xunit;

namespace Testably.Architecture.Testing.Tests;

public sealed partial class ExtensionsForITypeExpectationTests
{
	public sealed class BeAbstract
	{
		[Fact]
		public void ShouldBeAbstract_AbstractType_ShouldBeSatisfied()
		{
			Type type = typeof(AbstractType);
			var sut = Expect.That.Type(type);

			ITestResult<ITypeExpectation> result = sut.ShouldBeAbstract();

			result.IsSatisfied.Should().BeTrue();
		}

		[Fact]
		public void ShouldBeAbstract_ConcreteType_ShouldNotBeSatisfied()
		{
			Type type = typeof(ConcreteType);
			var sut = Expect.That.Type(type);

			ITestResult<ITypeExpectation> result = sut.ShouldBeAbstract();

			result.IsSatisfied.Should().BeFalse();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
			result.Errors[0].ToString().Should().Contain("should be abstract");
		}

		[Fact]
		public void ShouldNotBeAbstract_AbstractType_ShouldNotBeSatisfied()
		{
			Type type = typeof(AbstractType);
			var sut = Expect.That.Type(type);

			ITestResult<ITypeExpectation> result = sut.ShouldNotBeAbstract();

			result.IsSatisfied.Should().BeFalse();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
			result.Errors[0].ToString().Should().Contain("should not be abstract");
		}

		[Fact]
		public void ShouldNotBeAbstract_ConcreteType_ShouldBeSatisfied()
		{
			Type type = typeof(ConcreteType);
			var sut = Expect.That.Type(type);

			ITestResult<ITypeExpectation> result = sut.ShouldNotBeAbstract();

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
