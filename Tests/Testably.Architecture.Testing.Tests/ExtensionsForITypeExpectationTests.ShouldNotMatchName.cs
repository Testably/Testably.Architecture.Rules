using FluentAssertions;
using System;
using Testably.Architecture.Testing.TestErrors;
using Xunit;

namespace Testably.Architecture.Testing.Tests;

public sealed partial class ExtensionsForITypeExpectationTests
{
	public sealed class ShouldNotMatchName
	{
		[Theory]
		[InlineData(false)]
		[InlineData(true)]
		public void ShouldNotMatchName_IgnoreCase_ShouldConsiderParameter(bool ignoreCase)
		{
			Type type = typeof(ShouldNotMatchName);
			IFilterableTypeExpectation sut = Expect.That.Type(type);

			ITestResult<ITypeExpectation> result =
				sut.ShouldNotMatchName("shouldNOTmatchNAME", ignoreCase);

			result.IsSatisfied.Should().Be(!ignoreCase);
		}

		[Theory]
		[InlineData("ShouldNot*Foo")]
		[InlineData("ShouldNot??Name")]
		[InlineData("shouldnotmatchname")]
		public void ShouldNotMatchName_NotMatchingPattern_ShouldBeSatisfied(
			string notMatchingPattern)
		{
			Type type = typeof(ShouldNotMatchName);
			IFilterableTypeExpectation sut = Expect.That.Type(type);

			ITestResult<ITypeExpectation> result = sut.ShouldNotMatchName(notMatchingPattern);

			result.IsSatisfied.Should().BeTrue();
		}

		[Theory]
		[InlineData("ShouldNot*")]
		[InlineData("ShouldNot?????Name")]
		[InlineData("ShouldNotMatchName")]
		public void ShouldNotMatchName_MatchingPattern_ShouldNotBeSatisfied(string matchingPattern)
		{
			Type type = typeof(ShouldNotMatchName);
			IFilterableTypeExpectation sut = Expect.That.Type(type);

			ITestResult<ITypeExpectation> result = sut.ShouldNotMatchName(matchingPattern);

			result.IsSatisfied.Should().BeFalse();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.ToString().Should().Contain($"'{matchingPattern}'");
		}
	}
}
