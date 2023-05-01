using FluentAssertions;
using System;
using Testably.Architecture.Testing.TestErrors;
using Xunit;

namespace Testably.Architecture.Testing.Tests;

public sealed partial class ExtensionsForITypeExpectationTests
{
	public sealed class ShouldMatchName
	{
		[Theory]
		[InlineData(false)]
		[InlineData(true)]
		public void ShouldMatchName_IgnoreCase_ShouldConsiderParameter(bool ignoreCase)
		{
			Type type = typeof(ShouldMatchName);
			IFilterableTypeExpectation sut = Expect.That.Type(type);

			ITestResult<ITypeExpectation> result =
				sut.ShouldMatchName("shouldMATCHname", ignoreCase);

			result.IsSatisfied.Should().Be(ignoreCase);
		}

		[Theory]
		[InlineData("Should*Foo")]
		[InlineData("Should????Name")]
		[InlineData("shouldmatchname")]
		public void ShouldMatchName_InvalidPattern_ShouldNotBeSatisfied(string invalidPattern)
		{
			Type type = typeof(ShouldMatchName);
			IFilterableTypeExpectation sut = Expect.That.Type(type);

			ITestResult<ITypeExpectation> result = sut.ShouldMatchName(invalidPattern);

			result.IsSatisfied.Should().BeFalse();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.ToString().Should().Contain($"'{invalidPattern}'");
		}

		[Theory]
		[InlineData("Should*")]
		[InlineData("Should?????Name")]
		[InlineData("ShouldMatchName")]
		public void ShouldMatchName_ValidPattern_ShouldBeSatisfied(string validPattern)
		{
			Type type = typeof(ShouldMatchName);
			IFilterableTypeExpectation sut = Expect.That.Type(type);

			ITestResult<ITypeExpectation> result = sut.ShouldMatchName(validPattern);

			result.IsSatisfied.Should().BeTrue();
		}
	}
}
