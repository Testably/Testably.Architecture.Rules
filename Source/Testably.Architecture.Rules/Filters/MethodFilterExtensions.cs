using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     Extension methods for <see cref="IMethodFilter" />.
/// </summary>
public static partial class MethodFilterExtensions
{
	/// <summary>
	///     Filters the applicable <see cref="MethodInfo" /> on which the expectations should be applied.
	/// </summary>
	/// <param name="this">The <see cref="IMethodFilter" />.</param>
	/// <param name="filter">The filter to apply on the <see cref="MethodInfo" />.</param>
	public static IMethodFilterResult Which(this IMethodFilter @this,
		Expression<Func<MethodInfo, bool>> filter)
	{
		return @this.Which(Filter.FromPredicate(filter));
	}

	/// <summary>
	///     Filters the applicable <see cref="MethodInfo" /> on which the expectations should be applied.
	/// </summary>
	/// <param name="this">The <see cref="IMethodFilter" />.</param>
	/// <param name="filter">The filter to apply on the <see cref="MethodInfo" />.</param>
	/// <param name="name">The name of the filter.</param>
	public static IMethodFilterResult Which(this IMethodFilter @this,
		Func<MethodInfo, bool> filter,
		string name)
	{
		return @this.Which(Filter.FromPredicate(filter, name));
	}
}
