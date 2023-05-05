namespace Testably.Architecture.Rules;

/// <summary>
///     The result of a condition on <typeparamref name="TType" />.
/// </summary>
public interface IRequirementResult<TType> : IRule, IExemption<TType>
{
	/// <summary>
	///     Add additional conditions for the architecture expectation on <typeparamref name="TType" />.
	/// </summary>
	IRequirement<TType> And { get; }
}
