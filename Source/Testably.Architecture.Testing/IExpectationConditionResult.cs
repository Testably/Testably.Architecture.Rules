namespace Testably.Architecture.Testing;

/// <summary>
///     The result of a condition on <typeparamref name="TType" />.
/// </summary>
public interface IExpectationConditionResult<TType> : ITestResult, IExpectationExemption<TType>
{
	/// <summary>
	///     Add additional conditions for the architecture expectation on <typeparamref name="TType" />.
	/// </summary>
	IExpectationCondition<TType> And { get; }
}
