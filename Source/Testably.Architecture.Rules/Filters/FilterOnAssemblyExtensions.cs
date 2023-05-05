using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     Extension methods for <see cref="IFilter{Assembly}" />.
/// </summary>
public static class FilterOnAssemblyExtensions
{
	/// <summary>
	///     Filters the applicable <see cref="Assembly" /> on which the expectations should be applied.
	/// </summary>
	/// <param name="this">The <see cref="IFilter{Assembly}" />.</param>
	/// <param name="filter">The filter to apply on the <see cref="Assembly" />.</param>
	public static IFilterResult<Assembly> Which(this IFilter<Assembly> @this,
		Expression<Func<Assembly, bool>> filter)
	{
		return @this.Which(Filter.FromPredicate(filter));
	}

	/// <summary>
	///     Filters the applicable <see cref="Assembly" /> on which the expectations should be applied.
	/// </summary>
	/// <param name="this">The <see cref="IFilter{Assembly}" />.</param>
	/// <param name="filter">The filter to apply on the <see cref="Assembly" />.</param>
	/// <param name="name">The name of the filter.</param>
	public static IFilterResult<Assembly> Which(this IFilter<Assembly> @this,
		Func<Assembly, bool> filter,
		string name)
	{
		return @this.Which(Filter.FromPredicate(filter, name));
	}
}
