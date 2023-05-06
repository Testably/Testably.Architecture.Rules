using FluentAssertions;
using Xunit;

namespace Testably.Architecture.Rules.Tests;

public class ExpectTests
{
	[Fact]
	public void That_ShouldReturnDifferentInstances()
	{
		IExpectation result1 = Expect.That;
		IExpectation result2 = Expect.That;

		result1.Should().NotBe(result2);
	}
}
