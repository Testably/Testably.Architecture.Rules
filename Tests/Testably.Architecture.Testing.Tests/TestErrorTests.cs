using AutoFixture.Xunit2;
using FluentAssertions;
using Testably.Architecture.Testing.TestErrors;
using Xunit;

namespace Testably.Architecture.Testing.Tests;

public class TestErrorTests
{
	[Theory]
	[AutoData]
	public void ImplicitConversionToString_ShouldReturnErrorMessage(string errorMessage)
	{
		TestError sut = new(errorMessage);

		string result = sut;

		result.Should().Be(errorMessage);
	}

	[Theory]
	[AutoData]
	public void ToString_ShouldReturnErrorMessage(string errorMessage)
	{
		TestError sut = new(errorMessage);

		string result = sut.ToString();

		result.Should().Be(errorMessage);
	}
}
