using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     A <see cref="FieldInfo" /> for an expectation on an <see cref="TestError" />.
/// </summary>
public class FieldTestError : TestError
{
	/// <summary>
	///     The <see cref="FieldInfo" /> which does not satisfy all architecture expectations.
	/// </summary>
	public FieldInfo Field { get; }

	/// <summary>
	///     Initializes a new instance of <see cref="FieldTestError" />.
	/// </summary>
	/// <param name="field">
	///     The <see cref="FieldInfo" /> which does not satisfy all architecture expectations.
	/// </param>
	/// <param name="errorMessage">The error message for the <see cref="TestError" />.</param>
	public FieldTestError(FieldInfo field, string errorMessage)
		: base(errorMessage)
	{
		Field = field;
	}
}
