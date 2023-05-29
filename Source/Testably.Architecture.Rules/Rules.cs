using Testably.Architecture.Rules.Internal;

namespace Testably.Architecture.Rules;

/// <summary>
///     Bundle <see cref="IRule" />s together.
/// </summary>
public static class Rules
{
	/// <summary>
	///     Bundles <paramref name="rules" /> together under a <paramref name="name" />.
	/// </summary>
	public static IRule Bundle(string name, params IRule[] rules)
		=> new RuleBundle(name, rules);
}
