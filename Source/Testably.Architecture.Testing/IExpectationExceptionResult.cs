namespace Testably.Architecture.Testing;

/// <summary>
///     The result of a condition on <typeparamref name="TType" />.
/// </summary>
public interface IExpectationExceptionResult<TType> : ITestResult
{
	/// <summary>
	///     Add additional exception for <typeparamref name="TType" />.
	/// </summary>
	IExpectationResult<TType> And { get; }
}
