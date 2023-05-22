using FluentAssertions;
using System;
using Testably.Architecture.Rules.Internal;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Internal;

public sealed class LogHelperTests
{
	[Fact]
	public void Log_WithMultipleLines_ShouldIndentOtherLines()
	{
		string message = $"foo{Environment.NewLine}bar";
		string? receivedMessage = null;
		Action<string> sut = m => receivedMessage = m;

		sut.Log(message);

		receivedMessage.Should().Contain($"foo{Environment.NewLine}");
		receivedMessage.Should().Contain("              bar");
	}
}
