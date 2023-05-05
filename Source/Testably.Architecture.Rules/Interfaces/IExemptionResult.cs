namespace Testably.Architecture.Rules;

/// <summary>
///     The result of a condition on <typeparamref name="TType" />.
/// </summary>
public interface IExemptionResult<TType> : IRule
{
	/// <summary>
	///     Add additional exception for <typeparamref name="TType" />.
	/// </summary>
	IExemption<TType> And { get; }
}
