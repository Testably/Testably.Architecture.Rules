using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

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
			string message = result.ToString(ruleName);
			ThrowIfViolatedUsingXunit(message);

			throw new ArchitectureRuleViolatedException(message);
		}

		return result;
	}

	/// <summary>
	///     Try to throw the <paramref name="message" /> as a `XunitException`
	///     to improve readability in the test result window.
	/// </summary>
	/// <param name="message"></param>
	private static void ThrowIfViolatedUsingXunit(string message)
	{
		try
		{
			Type? exceptionType = Assembly
				.Load(new AssemblyName("xunit.assert"))
				.GetType("Xunit.Sdk.XunitException");

			if (exceptionType != null &&
			    Activator.CreateInstance(exceptionType, message) is Exception
				    exception)
			{
				throw exception;
			}
		}
		catch (FileNotFoundException)
		{
			// xunit.assert could not be loaded.
		}
	}
}
