using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     Extension methods for <see cref="IAssemblyFilter" />.
/// </summary>
public static class FilterOnAssemblyExtensions
{
	/// <summary>
	///     Filters the applicable <see cref="Assembly" /> on which the expectations should be applied.
	/// </summary>
	/// <param name="this">The <see cref="IAssemblyFilter" />.</param>
	/// <param name="filter">The filter to apply on the <see cref="Assembly" />.</param>
	public static IAssemblyFilterResult Which(this IAssemblyFilter @this,
		Expression<Func<Assembly, bool>> filter)
	{
		return @this.Which(Filter.FromPredicate(filter));
	}

	/// <summary>
	///     Filters the applicable <see cref="Assembly" /> on which the expectations should be applied.
	/// </summary>
	/// <param name="this">The <see cref="IAssemblyFilter" />.</param>
	/// <param name="filter">The filter to apply on the <see cref="Assembly" />.</param>
	/// <param name="name">The name of the filter.</param>
	public static IAssemblyFilterResult Which(this IAssemblyFilter @this,
		Func<Assembly, bool> filter,
		string name)
	{
		return @this.Which(Filter.FromPredicate(filter, name));
	}
}
