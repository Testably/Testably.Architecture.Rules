namespace Testably.Architecture.Rules;

/// <summary>
///     The architecture rule contains the <see cref="Filter" />s, <see cref="Requirement" />s
///     and <see cref="Exemption" />s.<br />
///     It can be checked against a set of <see cref="System.Reflection.Assembly" />s.
/// </summary>
public interface IRule
{
	/// <summary>
	///     Checks the <see cref="IRule" /> against a set of <see cref="System.Reflection.Assembly" />s.
	/// </summary>
	IRuleCheck Check { get; }
}
