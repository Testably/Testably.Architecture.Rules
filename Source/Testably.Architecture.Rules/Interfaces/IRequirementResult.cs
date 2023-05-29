namespace Testably.Architecture.Rules;

/// <summary>
///     The result of a condition on <typeparamref name="TEntity" />.
/// </summary>
public interface IRequirementResult<TEntity> : IRule, IExemption<TEntity>
{
	/// <summary>
	///     Add additional conditions for the architecture expectation on <typeparamref name="TEntity" />.
	/// </summary>
	IRequirement<TEntity> And { get; }
}
