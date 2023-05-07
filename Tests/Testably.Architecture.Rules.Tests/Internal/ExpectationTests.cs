using FluentAssertions;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Internal;

public sealed class ExpectationTests
{
	[Fact]
	public void Assemblies_ShouldReturnDifferentInstances()
	{
		IAssemblyExpectation result1 = Expect.That.Assemblies;
		IAssemblyExpectation result2 = Expect.That.Assemblies;

		result1.Should().NotBe(result2);
	}

	[Fact]
	public void Types_ShouldReturnDifferentInstances()
	{
		ITypeExpectation result1 = Expect.That.Types;
		ITypeExpectation result2 = Expect.That.Types;

		result1.Should().NotBe(result2);
	}
}
