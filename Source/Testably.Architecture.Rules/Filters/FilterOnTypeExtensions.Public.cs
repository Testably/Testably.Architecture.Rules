using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     Extension methods for <see cref="ITypeFilter" />.
/// </summary>
public static partial class FilterOnTypeExtensions
{
	/// <summary>
	///     Filter for not public types.
	/// </summary>
	public static ITypeFilterResult WhichAreNotPublic(
		this ITypeFilter @this)
	{
		return @this.Which(type => !type.IsPublic);
	}

	/// <summary>
	///     Filter for public types.
	/// </summary>
	public static ITypeFilterResult WhichArePublic(this ITypeFilter @this)
	{
		return @this.Which(type => type.IsPublic);
	}
}
