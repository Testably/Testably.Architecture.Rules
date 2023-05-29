namespace Testably.Architecture.Rules;

/// <summary>
///     Allows defining exceptions to expectations, by removing specific errors.
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IExemption<TEntity>
{
	/// <summary>
	///     Allows defining exceptions to expectations, by removing specific errors.
	/// </summary>
	/// <param name="exemption">Errors that match <paramref name="exemption" /> are allowed.</param>
	IExemptionResult<TEntity> Unless(Exemption exemption);
}
