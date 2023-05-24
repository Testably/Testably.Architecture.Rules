using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     Extension fields for <see cref="IFieldFilter" />.
/// </summary>
public static partial class FieldFilterExtensions
{
	/// <summary>
	///     Filters the applicable <see cref="FieldInfo" /> on which the expectations should be applied.
	/// </summary>
	/// <param name="this">The <see cref="IFieldFilter" />.</param>
	/// <param name="filter">The filter to apply on the <see cref="FieldInfo" />.</param>
	public static IFieldFilterResult Which(this IFieldFilter @this,
		Expression<Func<FieldInfo, bool>> filter)
	{
		return @this.Which(Filter.FromPredicate(filter));
	}

	/// <summary>
	///     Filters the applicable <see cref="FieldInfo" /> on which the expectations should be applied.
	/// </summary>
	/// <param name="this">The <see cref="IFieldFilter" />.</param>
	/// <param name="filter">The filter to apply on the <see cref="FieldInfo" />.</param>
	/// <param name="name">The name of the filter.</param>
	public static IFieldFilterResult Which(this IFieldFilter @this,
		Func<FieldInfo, bool> filter,
		string name)
	{
		return @this.Which(Filter.FromPredicate(filter, name));
	}
}
