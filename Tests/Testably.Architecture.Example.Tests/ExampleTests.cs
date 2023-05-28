using Testably.Architecture.Example.Tests.TestHelpers;
using Testably.Architecture.Rules;
using Xunit;

namespace Testably.Architecture.Example.Tests;

public sealed class ExampleTests
{
	/// <summary>
	///     All test classes should be named with 'Tests' as suffix.
	///     <para />
	///     Enforces this rule in assembly `Testably.Architecture.Rules.Tests`
	/// </summary>
	[Fact]
	public void ExpectTestClassesToBeNamedWithTestSuffix()
	{
		IRule rule = Expect.That.Types
			.WhichAreTestClasses()
			.ShouldMatchName("*Tests");

		rule.Check
			.InTestAssembly()
			.ThrowIfViolated();
	}

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

	/// <summary>
	///     Theories should have at least one parameter.
	/// </summary>
	[Fact]
	public void TheoriesShouldHaveParameters()
	{
		IRule rule = Expect.That.Methods
			.WithAttribute<TheoryAttribute>()
			.ShouldSatisfy(m => m.GetParameters().Length > 0);

		rule.Check
			.InTestAssembly()
			.ThrowIfViolated();
	}
}
