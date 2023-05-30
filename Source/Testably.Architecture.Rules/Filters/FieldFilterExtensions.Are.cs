using System.Reflection;

namespace Testably.Architecture.Rules;

public static partial class FieldFilterExtensions
{
	/// <summary>
	///     Filters for <see cref="FieldInfo" />s with the correct <paramref name="accessModifiers"/>.
	/// </summary>
	public static IFieldFilterResult WhichAre(
		this IFieldFilter @this,
		AccessModifiers accessModifiers)
	{
		return @this.Which(
			field => field.HasAccessModifier(accessModifiers),
			$"with {accessModifiers} access modifier");
	}
}
