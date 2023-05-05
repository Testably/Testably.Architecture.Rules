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
	///     Filter for not public types.
	/// </summary>
	public static IFilterResult<Type> WhichAreNotPublic(
		this IFilter<Type> @this)
	{
		return @this.Which(type => !type.IsPublic);
	}

	/// <summary>
	///     Filter for public types.
	/// </summary>
	public static IFilterResult<Type> WhichArePublic(this IFilter<Type> @this)
	{
		return @this.Which(type => type.IsPublic);
	}
}
