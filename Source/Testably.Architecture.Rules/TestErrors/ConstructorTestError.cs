using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     A <see cref="ConstructorInfo" /> for an expectation on an <see cref="TestError" />.
/// </summary>
public class ConstructorTestError : TestError
{
	/// <summary>
	///     The <see cref="ConstructorInfo" /> which does not satisfy all architecture expectations.
	/// </summary>
	public ConstructorInfo Constructor { get; }

	/// <summary>
	///     Initializes a new instance of <see cref="ConstructorTestError" />.
	/// </summary>
	/// <param name="constructor">
	///     The <see cref="ConstructorInfo" /> which does not satisfy all architecture expectations.
	/// </param>
	/// <param name="errorMessage">The error message for the <see cref="TestError" />.</param>
	public ConstructorTestError(ConstructorInfo constructor, string errorMessage)
		: base(errorMessage)
	{
		Constructor = constructor;
	}
}
