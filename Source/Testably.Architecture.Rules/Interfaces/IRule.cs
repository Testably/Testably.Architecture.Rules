namespace Testably.Architecture.Rules;

/// <summary>
///     The result of an architecture test.
/// </summary>
public interface IRule
{
	/// <summary>
	///     Allows checking the <see cref="IRule" />.
	/// </summary>
	IRuleCheck Check { get; }
}
