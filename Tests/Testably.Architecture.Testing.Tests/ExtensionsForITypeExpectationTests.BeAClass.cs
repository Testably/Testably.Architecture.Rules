using FluentAssertions;
using System;
using Testably.Architecture.Testing.TestErrors;
using Xunit;

namespace Testably.Architecture.Testing.Tests;

public sealed partial class ExtensionsForITypeExpectationTests
{
	public sealed class BeAClass
	{
		[Fact]
		public void ShouldBeAClass_ClassType_ShouldBeSatisfied()
		{
			Type type = typeof(ClassType);
			var sut = Expect.That.Type(type);

			ITestResult<IExpectationCondition<Type>> result = sut.ShouldBeAClass();

			result.IsSatisfied.Should().BeTrue();
		}

		[Fact]
		public void ShouldBeAClass_EnumType_ShouldNotBeSatisfied()
		{
			Type type = typeof(EnumType);
			var sut = Expect.That.Type(type);

			ITestResult<IExpectationCondition<Type>> result = sut.ShouldBeAClass();

			result.IsSatisfied.Should().BeFalse();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
			result.Errors[0].ToString().Should().Contain("should be a class");
		}

		[Fact]
		public void ShouldNotBeAClass_ClassType_ShouldNotBeSatisfied()
		{
			Type type = typeof(ClassType);
			var sut = Expect.That.Type(type);

			ITestResult<IExpectationCondition<Type>> result = sut.ShouldNotBeAClass();

			result.IsSatisfied.Should().BeFalse();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
			result.Errors[0].ToString().Should().Contain("should not be a class");
		}

		[Fact]
		public void ShouldNotBeAClass_EnumType_ShouldBeSatisfied()
		{
			Type type = typeof(EnumType);
			var sut = Expect.That.Type(type);

			ITestResult<IExpectationCondition<Type>> result = sut.ShouldNotBeAClass();

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
