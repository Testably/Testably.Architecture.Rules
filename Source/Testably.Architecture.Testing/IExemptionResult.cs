namespace Testably.Architecture.Testing;

/// <summary>
///     The result of a condition on <typeparamref name="TType" />.
/// </summary>
public interface IExemptionResult<TType> : ITestResult
{
	/// <summary>
	///     Add additional exception for <typeparamref name="TType" />.
	/// </summary>
	IExemption<TType> And { get; }
}
