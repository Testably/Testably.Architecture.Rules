using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     A <see cref="TestError" /> for a violated rule on a <see cref="PropertyInfo" />.
/// </summary>
public class PropertyTestError : TestError
{
	/// <summary>
	///     The <see cref="PropertyInfo" /> which does not satisfy all architecture rules.
	/// </summary>
	public PropertyInfo Property { get; }

	/// <summary>
	///     Initializes a new instance of <see cref="PropertyTestError" />.
	/// </summary>
	/// <param name="property">
	///     The <see cref="PropertyInfo" /> which does not satisfy all architecture rules.
	/// </param>
	/// <param name="errorMessage">The error message for the <see cref="TestError" />.</param>
	public PropertyTestError(PropertyInfo property, string errorMessage)
		: base(errorMessage)
	{
		Property = property;
	}
}
