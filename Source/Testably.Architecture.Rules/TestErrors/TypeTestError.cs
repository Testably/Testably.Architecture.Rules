using System;

namespace Testably.Architecture.Rules;

/// <summary>
///     A <see cref="Type" /> for an expectation on an <see cref="TestError" />.
/// </summary>
public class TypeTestError : TestError
{
	/// <summary>
	///     The <see cref="System.Type" /> which does not satisfy all architecture expectations.
	/// </summary>
	public Type Type { get; }

	/// <summary>
	///     Initializes a new instance of <see cref="TypeTestError" />.
	/// </summary>
	/// <param name="type">
	///     The <see cref="System.Type" /> which does not satisfy all architecture expectations.
	/// </param>
	/// <param name="errorMessage">The error message for the <see cref="TestError" />.</param>
	public TypeTestError(Type type, string errorMessage)
		: base(errorMessage)
	{
		Type = type;
	}
}
