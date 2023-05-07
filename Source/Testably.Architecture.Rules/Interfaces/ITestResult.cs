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
	///     Adds a human-readable description for the test result, which is included in <see cref="object.ToString()" />.
	/// </summary>
	/// <param name="description"></param>
	/// <returns></returns>
	ITestResult WithDescription(string description);
}
