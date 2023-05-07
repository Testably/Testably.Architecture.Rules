using System.Runtime.CompilerServices;
using Testably.Architecture.Rules.Internal;

namespace Testably.Architecture.Rules;

/// <summary>
///     Extension methods for <see cref="ITestResult" />.
/// </summary>
public static class TestResultExtensions
{
	/// <summary>
	///     Throws an exception if the <see cref="ITestResult" /> is violated.
	/// </summary>
	public static ITestResult ThrowIfViolated(this ITestResult result,
		[CallerMemberName] string ruleName = "")
	{
		if (result.IsViolated)
		{
			string message = result.WithDescription(ruleName).ToString()!;
			ThrowHelper.Throw(message);
		}

		return result;
	}
}
