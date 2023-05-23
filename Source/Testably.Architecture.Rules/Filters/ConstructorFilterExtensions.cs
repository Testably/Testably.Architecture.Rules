using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     Extension constructors for <see cref="IConstructorFilter" />.
/// </summary>
public static partial class ConstructorFilterExtensions
{
	/// <summary>
	///     Filters the applicable <see cref="ConstructorInfo" /> on which the expectations should be applied.
	/// </summary>
	/// <param name="this">The <see cref="IConstructorFilter" />.</param>
	/// <param name="filter">The filter to apply on the <see cref="ConstructorInfo" />.</param>
	public static IConstructorFilterResult Which(this IConstructorFilter @this,
		Expression<Func<ConstructorInfo, bool>> filter)
	{
		return @this.Which(Filter.FromPredicate(filter));
	}

	/// <summary>
	///     Filters the applicable <see cref="ConstructorInfo" /> on which the expectations should be applied.
	/// </summary>
	/// <param name="this">The <see cref="IConstructorFilter" />.</param>
	/// <param name="filter">The filter to apply on the <see cref="ConstructorInfo" />.</param>
	/// <param name="name">The name of the filter.</param>
	public static IConstructorFilterResult Which(this IConstructorFilter @this,
		Func<ConstructorInfo, bool> filter,
		string name)
	{
		return @this.Which(Filter.FromPredicate(filter, name));
	}
}
