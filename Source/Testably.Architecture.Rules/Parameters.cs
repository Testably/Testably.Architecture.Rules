using System.Reflection;
using Testably.Architecture.Rules.Internal;

namespace Testably.Architecture.Rules;

/// <summary>
///     Allows providing filters on an array of <see cref="ParameterInfo" />s of a <see cref="MethodInfo" />,
///     <see cref="ConstructorInfo" /> or <see cref="EventInfo" />.
/// </summary>
public static class Parameters
{
	/// <summary>
	///     Specifies a filter that must be satisfied by any <see cref="ParameterInfo" />.
	/// </summary>
	public static IParameterFilter<IUnorderedParameterFilterResult> Any
		=> new ParameterAnyFilter();

	/// <summary>
	///     Specifies an incrementing series of parameter filters starting at the first parameter that must be satisfied by the
	///     <see cref="ParameterInfo" />s in the correct order.
	/// </summary>
	public static IParameterFilter<IOrderedParameterFilterResult> First
		=> new ParameterInOrderFilter();

	/// <summary>
	///     Specifies a decrementing series of parameter filters starting at the last parameter that must be satisfied by the
	///     <see cref="ParameterInfo" />s in the correct order.
	/// </summary>
	public static IParameterFilter<IOrderedParameterFilterResult> Last
		=> new ParameterInOrderFilter().At(-1);
}
