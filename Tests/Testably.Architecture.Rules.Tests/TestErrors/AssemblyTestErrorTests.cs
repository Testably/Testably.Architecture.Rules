using AutoFixture.Xunit2;
using FluentAssertions;
using System.Reflection;
using Xunit;

namespace Testably.Architecture.Rules.Tests.TestErrors;

public sealed class AssemblyTestErrorTests
{
	[Fact]
	public void Constructor_ShouldSetAssembly()
	{
		Assembly assembly = Assembly.GetExecutingAssembly();

		AssemblyTestError sut = new(assembly, "foo");

		sut.Assembly.Should().BeSameAs(assembly);
	}

	[Theory]
	[AutoData]
	public void ToString_ShouldReturnMessage(string message)
	{
		Assembly assembly = Assembly.GetExecutingAssembly();

		AssemblyTestError sut = new(assembly, message);
		string result = sut.ToString();

		result.Should().Be(message);
	}
}
