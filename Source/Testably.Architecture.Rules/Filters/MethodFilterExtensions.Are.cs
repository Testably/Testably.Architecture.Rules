using System.Reflection;

namespace Testably.Architecture.Rules;

public static partial class MethodFilterExtensions
{
	/// <summary>
	///     Filters for <see cref="MethodInfo" />s with the correct <paramref name="accessModifiers"/>.
	/// </summary>
	public static IMethodFilterResult WhichAre(
		this IMethodFilter @this,
		AccessModifiers accessModifiers)
	{
		return @this.Which(
			method => method.HasAccessModifier(accessModifiers),
			$"with {accessModifiers} access modifier");
	}
}
