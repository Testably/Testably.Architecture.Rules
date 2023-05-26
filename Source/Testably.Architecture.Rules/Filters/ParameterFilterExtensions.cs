using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     Extension properties for <see cref="IParameterFilter{TResult}" />.
/// </summary>
public static partial class ParameterFilterExtensions
{
	/// <summary>
	///     Filters the applicable <see cref="ParameterInfo" /> on which the expectations should be applied.
	/// </summary>
	/// <param name="this">The <see cref="IParameterFilter{TResult}" />.</param>
	/// <param name="filter">The filter to apply on the <see cref="ParameterInfo" />.</param>
	public static TResult Which<TResult>(
		this IParameterFilter<TResult> @this,
		Expression<Func<ParameterInfo, bool>> filter)
		where TResult : IParameterFilterResult<TResult>
	{
		return @this.Which(Filter.FromPredicate(filter));
	}

	/// <summary>
	///     Filters the applicable <see cref="ParameterInfo" /> on which the expectations should be applied.
	/// </summary>
	/// <param name="this">The <see cref="IParameterFilter{TResult}" />.</param>
	/// <param name="filter">The filter to apply on the <see cref="ParameterInfo" />.</param>
	/// <param name="name">The name of the filter.</param>
	public static TResult Which<TResult>(
		this IParameterFilter<TResult> @this,
		Func<ParameterInfo, bool> filter,
		string name)
		where TResult : IParameterFilterResult<TResult>
	{
		return @this.Which(Filter.FromPredicate(filter, name));
	}
}
