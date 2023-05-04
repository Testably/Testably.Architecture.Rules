using AutoFixture.Xunit2;
using FluentAssertions;
using Xunit;

namespace Testably.Architecture.Testing.Tests.TestErrors;

public sealed class TestErrorTests
{
	[Theory]
	[AutoData]
	public void ImplicitOperatorToString_ShouldReturnMessage(string message)
	{
		TestError sut = new(message);

		string result = sut;

		result.Should().Be(message);
	}

	[Theory]
	[AutoData]
	public void ToString_ShouldReturnMessage(string message)
	{
		TestError sut = new(message);

		string result = sut.ToString();

		result.Should().Be(message);
	}

	[Theory]
	[AutoData]
	public void UpdateMessage_ShouldUpdateErrorMessage(string message,
		string updatedMessage)
	{
		TestErrorMock sut = new(message);

		sut.UpdateMessage(updatedMessage);

		sut.ToString().Should().Be(updatedMessage);
	}

	private sealed class TestErrorMock : TestError
	{
		public TestErrorMock(string message) : base(message)
		{
		}

		public new void UpdateMessage(string message)
			=> base.UpdateMessage(message);
	}
}
