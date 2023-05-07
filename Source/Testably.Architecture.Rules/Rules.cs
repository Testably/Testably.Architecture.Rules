using Testably.Architecture.Rules.Internal;

namespace Testably.Architecture.Rules;

/// <summary>
///     Allows bundling of <see cref="IRule" />s.
/// </summary>
public static class Rules
{
	/// <summary>
	///     Bundles some <paramref name="rules" /> together under a <paramref name="name" />.
	/// </summary>
	public static IRule Bundle(string name, params IRule[] rules)
		=> new RuleBundle(name, rules);
}
