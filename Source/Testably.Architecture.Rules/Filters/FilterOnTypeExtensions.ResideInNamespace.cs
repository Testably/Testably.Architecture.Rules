﻿namespace Testably.Architecture.Rules;

public static partial class FilterOnTypeExtensions
{
	/// <summary>
	///     Filter for types where the <see cref="System.Type.Namespace" /> matches the given <paramref name="pattern" />.
	/// </summary>
	public static ITypeFilterResult WhichResideInNamespace(this ITypeFilter @this,
		Match pattern,
		bool ignoreCase = false)
	{
		return @this.Which(type => pattern.Matches(type.Namespace, ignoreCase));
	}

	/// <summary>
	///     Filter for types where the <see cref="System.Type.Namespace" /> does not match the given
	///     <paramref name="pattern" />.
	/// </summary>
	public static ITypeFilterResult WhichDoNotResideInNamespace(this ITypeFilter @this,
		Match pattern,
		bool ignoreCase = false)
	{
		return @this.Which(type => !pattern.Matches(type.Namespace, ignoreCase));
	}
}