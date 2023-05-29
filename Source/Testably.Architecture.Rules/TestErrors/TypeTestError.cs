using System;

namespace Testably.Architecture.Rules;

/// <summary>
///     A <see cref="TestError" /> for a violated rule on a <see cref="System.Type" />.
/// </summary>
public class TypeTestError : TestError
{
	/// <summary>
	///     The <see cref="System.Type" /> which does not satisfy all architecture rules.
	/// </summary>
	public Type Type { get; }

	/// <summary>
	///     Initializes a new instance of <see cref="TypeTestError" />.
	/// </summary>
	/// <param name="type">
	///     The <see cref="System.Type" /> which does not satisfy all architecture rules.
	/// </param>
	/// <param name="errorMessage">The error message for the <see cref="TestError" />.</param>
	public TypeTestError(Type type, string errorMessage)
		: base(errorMessage)
	{
		Type = type;
	}
}
