using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     A <see cref="TestError" /> for a violated rule on a <see cref="FieldInfo" />.
/// </summary>
public class FieldTestError : TestError
{
	/// <summary>
	///     The <see cref="FieldInfo" /> which does not satisfy all architecture rules.
	/// </summary>
	public FieldInfo Field { get; }

	/// <summary>
	///     Initializes a new instance of <see cref="FieldTestError" />.
	/// </summary>
	/// <param name="field">
	///     The <see cref="FieldInfo" /> which does not satisfy all architecture rules.
	/// </param>
	/// <param name="errorMessage">The error message for the <see cref="TestError" />.</param>
	public FieldTestError(FieldInfo field, string errorMessage)
		: base(errorMessage)
	{
		Field = field;
	}
}
