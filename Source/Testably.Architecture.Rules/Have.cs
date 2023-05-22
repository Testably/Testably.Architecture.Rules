using Testably.Architecture.Rules.Internal;

namespace Testably.Architecture.Rules;

/// <summary>
///     Allows providing filters on elements of a <see cref="System.Type" />.
/// </summary>
public static class Have
{
	/// <summary>
	///     Applies conditions on the <see cref="System.Type.GetConstructors()" />.
	/// </summary>
	public static IConstructorFilter Constructor
		=> new ConstructorFilter();

	/// <summary>
	///     Applies conditions on the <see cref="System.Type.GetMethods()" />.
	/// </summary>
	public static IMethodFilter Method
		=> new MethodFilter();
}
