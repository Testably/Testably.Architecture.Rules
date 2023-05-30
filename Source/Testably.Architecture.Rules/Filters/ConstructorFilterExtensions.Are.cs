using System.Reflection;

namespace Testably.Architecture.Rules;

public static partial class ConstructorFilterExtensions
{
	/// <summary>
	///     Filters for <see cref="ConstructorInfo" />s with the correct <paramref name="accessModifiers" />.
	/// </summary>
	public static IConstructorFilterResult WhichAre(
		this IConstructorFilter @this,
		AccessModifiers accessModifiers)
	{
		return @this.Which(
			constructor => constructor.HasAccessModifier(accessModifiers),
			$"with {accessModifiers} access modifier");
	}
}
