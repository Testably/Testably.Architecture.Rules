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
			var sut = Expect.That.Type(type);

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
			var sut = Expect.That.Type(type);

			IExpectationResult<Type> result = sut.ShouldBeStatic();

			result.IsSatisfied.Should().BeTrue();
		}

		[Fact]
		public void ShouldNotBeStatic_InstanceType_ShouldBeSatisfied()
		{
			Type type = typeof(InstanceType);
			var sut = Expect.That.Type(type);

			IExpectationResult<Type> result = sut.ShouldNotBeStatic();

			result.IsSatisfied.Should().BeTrue();
		}

		[Fact]
		public void ShouldNotBeStatic_StaticType_ShouldNotBeSatisfied()
		{
			Type type = typeof(StaticType);
			var sut = Expect.That.Type(type);

			IExpectationResult<Type> result = sut.ShouldNotBeStatic();

			result.IsSatisfied.Should().BeFalse();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
			result.Errors[0].ToString().Should().Contain("should not be static");
		}

		private abstract class InstanceType
		{
		}

		private static class StaticType
		{
		}
	}
}
