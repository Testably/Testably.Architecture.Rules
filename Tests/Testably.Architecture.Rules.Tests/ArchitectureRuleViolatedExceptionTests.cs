using AutoFixture.Xunit2;
using FluentAssertions;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Xunit;

namespace Testably.Architecture.Rules.Tests;

public sealed class ArchitectureRuleViolatedExceptionTests
{
	[Theory]
	[AutoData]
	public void Constructor_WithMessageParameter_ShouldForwardToException(
		string message)
	{
		Exception sut = new ArchitectureRuleViolatedException(message);

		sut.Message.Should().Be(message);
		sut.InnerException.Should().BeNull();
	}

	[Theory]
	[AutoData]
	public void Serialization_Deserialization_ShouldPersistExceptionDetails(
		string message)
	{
		ArchitectureRuleViolatedException originalException = new(message);
		byte[] buffer = new byte[4096];
		MemoryStream ms = new(buffer);
		MemoryStream ms2 = new(buffer);

		#pragma warning disable SYSLIB0011
		BinaryFormatter formatter = new();
		formatter.Serialize(ms, originalException);
		ArchitectureRuleViolatedException deserializedException =
			(ArchitectureRuleViolatedException)formatter.Deserialize(ms2);
		#pragma warning restore SYSLIB0011

		Assert.Equal(originalException.Message, deserializedException.Message);
	}
}
