namespace Testably.Architecture.Testing;

/// <summary>
///     The result of a condition on <typeparamref name="TType" />.
/// </summary>
public interface IRequirementResult<TType> : ITestResult, IExemption<TType>
{
	/// <summary>
	///     Add additional conditions for the architecture expectation on <typeparamref name="TType" />.
	/// </summary>
	IRequirement<TType> And { get; }
}
