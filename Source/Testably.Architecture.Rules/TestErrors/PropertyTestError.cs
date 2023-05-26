using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     A <see cref="PropertyInfo" /> for an expectation on an <see cref="TestError" />.
/// </summary>
public class PropertyTestError : TestError
{
	/// <summary>
	///     The <see cref="PropertyInfo" /> which does not satisfy all architecture expectations.
	/// </summary>
	public PropertyInfo Property { get; }

	/// <summary>
	///     Initializes a new instance of <see cref="PropertyTestError" />.
	/// </summary>
	/// <param name="property">
	///     The <see cref="PropertyInfo" /> which does not satisfy all architecture expectations.
	/// </param>
	/// <param name="errorMessage">The error message for the <see cref="TestError" />.</param>
	public PropertyTestError(PropertyInfo property, string errorMessage)
		: base(errorMessage)
	{
		Property = property;
	}
}
