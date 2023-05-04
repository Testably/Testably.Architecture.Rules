namespace Testably.Architecture.Testing;

/// <summary>
///     The result of a condition on <typeparamref name="TType" />.
/// </summary>
public interface IExpectationExemptionResult<TType> : ITestResult
{
	/// <summary>
	///     Add additional exception for <typeparamref name="TType" />.
	/// </summary>
	IExpectationExemption<TType> And { get; }
}
