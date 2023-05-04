using AutoFixture.Xunit2;
using FluentAssertions;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Xunit;

namespace Testably.Architecture.Testing.Tests;

public class EmptyDataExceptionTests
{
	[Theory]
	[AutoData]
	public void Constructor_WithMessageParameter_ShouldForwardToException(
		string message)
	{
		Exception sut = new EmptyDataException(message);

		sut.Message.Should().Be(message);
		sut.InnerException.Should().BeNull();
	}

	[Theory]
	[AutoData]
	public void Constructor_WithInnerException_ShouldForwardToException(
		string message, Exception innerException)
	{
		Exception sut = new EmptyDataException(message, innerException);

		sut.Message.Should().Be(message);
		sut.InnerException.Should().Be(innerException);
	}

	[Theory]
	[AutoData]
	public void Serialization_Deserialization_ShouldPersistExceptionDetails(
		string message, Exception innerException)
	{
		EmptyDataException originalException = new(message, innerException);
		byte[] buffer = new byte[4096];
		MemoryStream ms = new(buffer);
		MemoryStream ms2 = new(buffer);

		#pragma warning disable SYSLIB0011
		BinaryFormatter formatter = new();
		formatter.Serialize(ms, originalException);
		EmptyDataException deserializedException = (EmptyDataException)formatter.Deserialize(ms2);
		#pragma warning restore SYSLIB0011

		Assert.Equal(innerException.Message, deserializedException.InnerException?.Message);
		Assert.Equal(originalException.Message, deserializedException.Message);
	}
}
