﻿using FluentAssertions;
using System;
using Testably.Architecture.Testing.TestErrors;
using Xunit;

namespace Testably.Architecture.Testing.Tests;

public sealed partial class ExtensionsForITypeExpectationTests
{
	public sealed class MatchName
	{
		[Theory]
		[InlineData(false)]
		[InlineData(true)]
		public void ShouldMatchName_IgnoreCase_ShouldConsiderParameter(bool ignoreCase)
		{
			Type type = typeof(MatchName);
			IFilterableTypeExpectation sut = Expect.That.Type(type);

			ITestResult<ITypeExpectation> result =
				sut.ShouldMatchName("MATCHname", ignoreCase);

			result.IsSatisfied.Should().Be(ignoreCase);
		}

		[Theory]
		[InlineData("*")]
		[InlineData("?????Name")]
		[InlineData("MatchName")]
		public void ShouldMatchName_MatchingPattern_ShouldBeSatisfied(string matchingPattern)
		{
			Type type = typeof(MatchName);
			IFilterableTypeExpectation sut = Expect.That.Type(type);

			ITestResult<ITypeExpectation> result = sut.ShouldMatchName(matchingPattern);

			result.IsSatisfied.Should().BeTrue();
		}

		[Theory]
		[InlineData("*Foo")]
		[InlineData("??Name")]
		[InlineData("matchname")]
		public void ShouldMatchName_NotMatchingPattern_ShouldNotBeSatisfied(
			string notMatchingPattern)
		{
			Type type = typeof(MatchName);
			IFilterableTypeExpectation sut = Expect.That.Type(type);

			ITestResult<ITypeExpectation> result = sut.ShouldMatchName(notMatchingPattern);

			result.IsSatisfied.Should().BeFalse();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.ToString().Should().Contain($"'{notMatchingPattern}'");
		}

		[Theory]
		[InlineData(false)]
		[InlineData(true)]
		public void ShouldNotMatchName_IgnoreCase_ShouldConsiderParameter(bool ignoreCase)
		{
			Type type = typeof(MatchName);
			IFilterableTypeExpectation sut = Expect.That.Type(type);

			ITestResult<ITypeExpectation> result =
				sut.ShouldNotMatchName("matchNAME", ignoreCase);

			result.IsSatisfied.Should().Be(!ignoreCase);
		}

		[Theory]
		[InlineData("*")]
		[InlineData("?????Name")]
		[InlineData("MatchName")]
		public void ShouldNotMatchName_MatchingPattern_ShouldNotBeSatisfied(string matchingPattern)
		{
			Type type = typeof(MatchName);
			IFilterableTypeExpectation sut = Expect.That.Type(type);

			ITestResult<ITypeExpectation> result = sut.ShouldNotMatchName(matchingPattern);

			result.IsSatisfied.Should().BeFalse();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.ToString().Should().Contain($"'{matchingPattern}'");
		}

		[Theory]
		[InlineData("*Foo")]
		[InlineData("??Name")]
		[InlineData("matchname")]
		public void ShouldNotMatchName_NotMatchingPattern_ShouldBeSatisfied(
			string notMatchingPattern)
		{
			Type type = typeof(MatchName);
			IFilterableTypeExpectation sut = Expect.That.Type(type);

			ITestResult<ITypeExpectation> result = sut.ShouldNotMatchName(notMatchingPattern);

			result.IsSatisfied.Should().BeTrue();
		}
	}
}
