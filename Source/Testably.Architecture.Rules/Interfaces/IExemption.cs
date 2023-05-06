namespace Testably.Architecture.Rules;

/// <summary>
///     Allows defining exceptions to expectations, by removing specific errors.
/// </summary>
/// <typeparam name="TType"></typeparam>
public interface IExemption<TType>
{
	/// <summary>
	///     Allows defining exceptions to expectations, by removing specific errors.
	/// </summary>
	/// <param name="exemption">Errors that match <paramref name="exemption" /> are allowed.</param>
	IExemptionResult<TType> Unless(Exemption exemption);
}
