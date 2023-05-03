using FluentAssertions;
using System;
using Testably.Architecture.Testing.TestErrors;
using Xunit;

namespace Testably.Architecture.Testing.Tests;

public sealed partial class ExtensionsForITypeExpectationTests
{
	public sealed class BeStatic
	{
		[Fact]
		public void ShouldBeStatic_InstanceType_ShouldNotBeSatisfied()
		{
			Type type = typeof(InstanceType);
			ITypeExpectation sut = Expect.That.Type(type);

			IExpectationResult<Type> result = sut.ShouldBeStatic();

			result.IsSatisfied.Should().BeFalse();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
			result.Errors[0].ToString().Should().Contain("should be static");
		}

		[Fact]
		public void ShouldBeStatic_InterfaceType_ShouldNotBeSatisfied()
		{
			Type type = typeof(IInterfaceType);
			ITypeExpectation sut = Expect.That.Type(type);

			IExpectationResult<Type> result = sut.ShouldBeStatic();

			result.IsSatisfied.Should().BeFalse();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
			result.Errors[0].ToString().Should().Contain("should be static");
		}

		[Fact]
		public void ShouldBeStatic_StaticType_ShouldBeSatisfied()
		{
			Type type = typeof(StaticType);
			ITypeExpectation sut = Expect.That.Type(type);

			IExpectationResult<Type> result = sut.ShouldBeStatic();

			result.IsSatisfied.Should().BeTrue();
		}

		[Fact]
		public void ShouldNotBeStatic_InstanceType_ShouldBeSatisfied()
		{
			Type type = typeof(InstanceType);
			ITypeExpectation sut = Expect.That.Type(type);

			IExpectationResult<Type> result = sut.ShouldNotBeStatic();

			result.IsSatisfied.Should().BeTrue();
		}

		[Fact]
		public void ShouldNotBeStatic_InterfaceType_ShouldBeSatisfied()
		{
			Type type = typeof(IInterfaceType);
			ITypeExpectation sut = Expect.That.Type(type);

			IExpectationResult<Type> result = sut.ShouldNotBeStatic();

			result.IsSatisfied.Should().BeTrue();
		}

		[Fact]
		public void ShouldNotBeStatic_StaticType_ShouldNotBeSatisfied()
		{
			Type type = typeof(StaticType);
			ITypeExpectation sut = Expect.That.Type(type);

			IExpectationResult<Type> result = sut.ShouldNotBeStatic();

			result.IsSatisfied.Should().BeFalse();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
			result.Errors[0].ToString().Should().Contain("should not be static");
		}

		private interface IInterfaceType
		{
		}

		private abstract class InstanceType
		{
		}

		private static class StaticType
		{
		}
	}
}
