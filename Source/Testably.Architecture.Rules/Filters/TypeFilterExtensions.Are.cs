using System;

namespace Testably.Architecture.Rules;

public static partial class TypeFilterExtensions
{
	/// <summary>
	///     Filters for <see cref="Type" />s with the correct <paramref name="accessModifiers" />.
	/// </summary>
	public static ITypeFilterResult WhichAre(
		this ITypeFilter @this,
		AccessModifiers accessModifiers)
	{
		return @this.Which(
			type => type.HasAccessModifier(accessModifiers),
			$"with {accessModifiers} access modifier");
	}
}
