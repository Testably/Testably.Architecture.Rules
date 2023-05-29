namespace Testably.Architecture.Rules;

/// <summary>
///     The result of an architecture test.
/// </summary>
public interface ITestResult
{
	/// <summary>
	///     The test errors when checking a <see cref="IRule" /> against
	///     a set of <see cref="System.Reflection.Assembly" />s.
	/// </summary>
	TestError[] Errors { get; }

	/// <summary>
	///     Flag indicating, if any rule was violated.<br />
	///     This flag is <see langword="true" /> exactly if <see cref="Errors" /> is not empty.
	/// </summary>
	bool IsViolated { get; }

	/// <summary>
	///     Adds a human-readable description for the test result, which is included in <see cref="object.ToString()" />.
	/// </summary>
	ITestResult WithDescription(string description);
}
