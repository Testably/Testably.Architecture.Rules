using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;

namespace Testably.Architecture.Rules.Internal;

internal static class ThrowHelper
{
	/// <summary>
	///     Throw an exception containing the <paramref name="message" />.
	/// </summary>
	[DoesNotReturn]
	[ExcludeFromCodeCoverage]
	public static void Throw(string message)
	{
		ThrowIfViolatedUsingXunit(message);
		throw new ArchitectureRuleViolatedException(message);
	}

	/// <summary>
	///     Try to throw the <paramref name="message" /> as a `XunitException`
	///     to improve readability in the test result window.
	/// </summary>
	/// <param name="message"></param>
	[ExcludeFromCodeCoverage]
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
