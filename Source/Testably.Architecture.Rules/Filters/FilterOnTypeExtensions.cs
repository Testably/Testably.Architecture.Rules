using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     Extension methods for <see cref="IFilter{Type}" />.
/// </summary>
public static partial class FilterOnTypeExtensions
{
	/// <summary>
	///     Filters the applicable <see cref="Type" /> on which the expectations should be applied.
	/// </summary>
	/// <param name="this">The <see cref="IFilter{Type}" />.</param>
	/// <param name="filter">The filter to apply on the <see cref="Type" />.</param>
	public static IFilterResult<Type> Which(this IFilter<Type> @this,
		Expression<Func<Type, bool>> filter)
	{
		return @this.Which(Filter.FromPredicate(filter));
	}

	/// <summary>
	///     Filters the applicable <see cref="Type" /> on which the expectations should be applied.
	/// </summary>
	/// <param name="this">The <see cref="IFilter{Type}" />.</param>
	/// <param name="filter">The filter to apply on the <see cref="Type" />.</param>
	/// <param name="name">The name of the filter.</param>
	public static IFilterResult<Type> Which(this IFilter<Type> @this,
		Func<Type, bool> filter,
		string name)
	{
		return @this.Which(Filter.FromPredicate(filter, name));
	}
}
