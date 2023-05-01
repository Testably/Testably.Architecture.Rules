using FluentAssertions;
using System;
using Testably.Architecture.Testing.TestErrors;
using Xunit;

namespace Testably.Architecture.Testing.Tests;

public sealed partial class ExtensionsForITypeExpectationTests
{
	public sealed class BeNested
	{
		[Fact]
		public void ShouldBeNested_NestedType_ShouldBeSatisfied()
		{
			Type type = typeof(NestedType);
			IFilterableTypeExpectation sut = Expect.That.Type(type);

			ITestResult<ITypeExpectation> result = sut.ShouldBeNested();

			result.IsSatisfied.Should().BeTrue();
		}

		[Fact]
		public void ShouldBeNested_UnnestedType_ShouldNotBeSatisfied()
		{
			Type type = typeof(ExtensionsForITypeExpectationTests);
			IFilterableTypeExpectation sut = Expect.That.Type(type);

			ITestResult<ITypeExpectation> result = sut.ShouldBeNested();

			result.IsSatisfied.Should().BeFalse();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
			result.Errors[0].ToString().Should().Contain("should be nested");
		}

		[Fact]
		public void ShouldNotBeNested_NestedType_ShouldNotBeSatisfied()
		{
			Type type = typeof(NestedType);
			IFilterableTypeExpectation sut = Expect.That.Type(type);

			ITestResult<ITypeExpectation> result = sut.ShouldNotBeNested();

			result.IsSatisfied.Should().BeFalse();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
			result.Errors[0].ToString().Should().Contain("should not be nested");
		}

		[Fact]
		public void ShouldNotBeNested_UnnestedType_ShouldBeSatisfied()
		{
			Type type = typeof(ExtensionsForITypeExpectationTests);
			IFilterableTypeExpectation sut = Expect.That.Type(type);

			ITestResult<ITypeExpectation> result = sut.ShouldNotBeNested();

			result.IsSatisfied.Should().BeTrue();
		}

		public class NestedType
		{
		}
	}
}
