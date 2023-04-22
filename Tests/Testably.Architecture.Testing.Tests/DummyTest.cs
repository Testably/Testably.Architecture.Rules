using FluentAssertions;
using Xunit;

namespace Testably.Architecture.Testing.Tests;

public class DummyTest
{
	[Fact]
	public void Dummy()
	{
		//This is a dummy test!

		bool result = true;

		result.Should().BeTrue();
	}
}
