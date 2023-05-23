using System;

namespace Testably.Architecture.Rules;

public static partial class TypeFilterExtensions
{
	/// <summary>
	///     Filter for types that don't inherit from <typeparamref name="TBase" />.
	/// </summary>
	public static ITypeFilterResult WhichDoNotInheritFrom<TBase>(this ITypeFilter @this,
		bool forceDirect = false)
	{
		return @this.WhichDoNotInheritFrom(typeof(TBase), forceDirect);
	}

	/// <summary>
	///     Filter for types that don't inherit from <paramref name="baseType" />.
	/// </summary>
	public static ITypeFilterResult WhichDoNotInheritFrom(this ITypeFilter @this, Type baseType,
		bool forceDirect = false)
	{
		return @this.Which(type => !type.InheritsFrom(baseType, forceDirect));
	}

	/// <summary>
	///     Filter for types that inherit from <typeparamref name="TBase" />.
	/// </summary>
	public static ITypeFilterResult WhichInheritFrom<TBase>(this ITypeFilter @this,
		bool forceDirect = false)
	{
		return @this.WhichInheritFrom(typeof(TBase), forceDirect);
	}

	/// <summary>
	///     Filter for types that inherit from <paramref name="baseType" />.
	/// </summary>
	public static ITypeFilterResult WhichInheritFrom(this ITypeFilter @this, Type baseType,
		bool forceDirect = false)
	{
		return @this.Which(type => type.InheritsFrom(baseType, forceDirect));
	}
}
