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
		=> At(0);

	/// <summary>
	///     Specifies a decrementing series of parameter filters starting at the last parameter that must be satisfied by the
	///     <see cref="ParameterInfo" />s in the correct order.
	/// </summary>
	public static IParameterFilter<IOrderedParameterFilterResult> Last
		=> At(-1);

	/// <summary>
	///     Specifies a series of parameter filters starting at the specified <paramref name="position" /> that must be
	///     satisfied by the
	///     <see cref="ParameterInfo" />s in the correct order.
	///     <para />
	///     Non-negative values start an incrementing series from a zero-based index from the start.<br />
	///     Negative values start a decrementing series from the last parameter back (e.g. `-1` indicates the last parameter).
	/// </summary>
	public static IParameterFilter<IOrderedParameterFilterResult> At(int position)
		=> new ParameterInOrderFilter().At(position);
}
