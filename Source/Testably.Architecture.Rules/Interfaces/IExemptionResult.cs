namespace Testably.Architecture.Rules;

/// <summary>
///     The result of a condition on <typeparamref name="TEntity" />.
/// </summary>
public interface IExemptionResult<TEntity> : IRule
{
	/// <summary>
	///     Add additional exception for <typeparamref name="TEntity" />.
	/// </summary>
	IExemption<TEntity> And { get; }
}
