using System;

namespace Testably.Architecture.Rules;

public static partial class FilterOnTypeExtensions
{
	/// <summary>
	///     Filter for types that implement the interface <typeparamref name="TInterface" />.
	/// </summary>
	public static ITypeFilterResult WhichImplement<TInterface>(this ITypeFilter @this,
		bool forceDirect = false)
	{
		return @this.WhichImplement(typeof(TInterface), forceDirect);
	}

	/// <summary>
	///     Filter for types that implement the interface <paramref name="interfaceType" />.
	/// </summary>
	public static ITypeFilterResult WhichImplement(this ITypeFilter @this,
		Type interfaceType,
		bool forceDirect = false)
	{
		return @this.Which(type => type.Implements(interfaceType, forceDirect));
	}

	/// <summary>
	///     Filter for types that don't implement the interface <typeparamref name="TInterface" />.
	/// </summary>
	public static ITypeFilterResult WhichDoNotImplement<TInterface>(this ITypeFilter @this,
		bool forceDirect = false)
	{
		return @this.WhichDoNotImplement(typeof(TInterface), forceDirect);
	}

	/// <summary>
	///     Filter for types that don't implement the interface <paramref name="interfaceType" />.
	/// </summary>
	public static ITypeFilterResult WhichDoNotImplement(this ITypeFilter @this,
		Type interfaceType,
		bool forceDirect = false)
	{
		return @this.Which(type => !type.Implements(interfaceType, forceDirect));
	}
}
