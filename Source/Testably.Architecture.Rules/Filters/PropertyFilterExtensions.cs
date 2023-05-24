using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     Extension properties for <see cref="IPropertyFilter" />.
/// </summary>
public static partial class PropertyFilterExtensions
{
	/// <summary>
	///     Filters the applicable <see cref="PropertyInfo" /> on which the expectations should be applied.
	/// </summary>
	/// <param name="this">The <see cref="IPropertyFilter" />.</param>
	/// <param name="filter">The filter to apply on the <see cref="PropertyInfo" />.</param>
	public static IPropertyFilterResult Which(this IPropertyFilter @this,
		Expression<Func<PropertyInfo, bool>> filter)
	{
		return @this.Which(Filter.FromPredicate(filter));
	}

	/// <summary>
	///     Filters the applicable <see cref="PropertyInfo" /> on which the expectations should be applied.
	/// </summary>
	/// <param name="this">The <see cref="IPropertyFilter" />.</param>
	/// <param name="filter">The filter to apply on the <see cref="PropertyInfo" />.</param>
	/// <param name="name">The name of the filter.</param>
	public static IPropertyFilterResult Which(this IPropertyFilter @this,
		Func<PropertyInfo, bool> filter,
		string name)
	{
		return @this.Which(Filter.FromPredicate(filter, name));
	}
}
