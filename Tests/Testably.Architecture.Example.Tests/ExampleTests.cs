using Testably.Architecture.Example.Tests.TestHelpers;
using Testably.Architecture.Rules;
using Xunit;

namespace Testably.Architecture.Example.Tests;

public sealed class ExampleTests
{
	/// <summary>
	///     All test classes should be sealed.
	///     <para />
	///     Enforces this rule in assembly `Testably.Architecture.Rules.Tests`
	/// </summary>
	[Fact]
	public void ExpectTestClassesToBeSealed()
	{
		IRule rule = Expect.That.Types
			.WhichAreTestClasses()
			.ShouldBeSealed();

		rule.Check
			.InTestAssembly()
			.ThrowIfViolated();
	}
}
