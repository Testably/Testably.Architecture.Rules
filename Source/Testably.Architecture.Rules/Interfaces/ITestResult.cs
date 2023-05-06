namespace Testably.Architecture.Rules;

/// <summary>
///     The result of an architecture test.
/// </summary>
public interface ITestResult
{
	/// <summary>
	///     The errors.
	/// </summary>
	TestError[] Errors { get; }

	/// <summary>
	///     Flag indicating, if all expectations were satisfied.
	/// </summary>
	bool IsViolated { get; }

	/// <summary>
	///     Create a human-readable message for the test result.
	/// </summary>
	/// <param name="ruleName">The default name of the rule.</param>
	string ToString(string ruleName);
}
