using Testably.Architecture.Rules;
using Testably.Architecture.Rules.Tests;
using Xunit;

namespace Testably.Architecture.Example.Tests.TestHelpers;

public static class ExampleTestExtensions
{
	/// <summary>
	///     Defines expectations on the Testably.Architecture.Rules.Tests assembly.
	/// </summary>
	public static ITestResult InTestAssembly(this IRuleCheck @this)
	{
		return @this.In(typeof(MatchTests).Assembly);
	}

	/// <summary>
	///     Test classes for `xunit` can be identified by having methods with
	///     either a <see cref="FactAttribute" /> or a <see cref="TheoryAttribute" />.<br />
	/// </summary>
	public static ITypeFilterResult WhichAreTestClasses(this ITypeFilter @this)
	{
		return @this.Which(
			Have.Method.WithAttribute<FactAttribute>().OrAttribute<TheoryAttribute>());
	}
}
