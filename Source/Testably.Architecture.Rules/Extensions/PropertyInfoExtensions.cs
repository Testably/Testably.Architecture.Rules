using System;
using System.Linq;
using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     Extension properties for <see cref="PropertyInfo" />.
/// </summary>
public static class PropertyInfoExtensions
{
	///// <summary>
	/////     Checks if the <paramref name="propertyInfo" /> has the specified <paramref name="accessModifiers" />.
	///// </summary>
	///// <param name="propertyInfo">The <see cref="PropertyInfo" /> which is checked to have the attribute.</param>
	///// <param name="accessModifiers">
	/////     The <see cref="AccessModifiers" />.
	/////     <para />
	/////     Supports specifying multiple <see cref="AccessModifiers" />.
	///// </param>
	//public static bool HasAccessModifier(
	//	this PropertyInfo propertyInfo,
	//	AccessModifiers accessModifiers)
	//{
	//	if (propertyInfo.IsAssembly)
	//	{
	//		return accessModifiers.HasFlag(AccessModifiers.Internal);
	//	}

	//	if (propertyInfo.IsFamily)
	//	{
	//		return accessModifiers.HasFlag(AccessModifiers.Protected);
	//	}

	//	if (propertyInfo.IsPrivate)
	//	{
	//		return accessModifiers.HasFlag(AccessModifiers.Private);
	//	}

	//	if (propertyInfo.IsPublic)
	//	{
	//		return accessModifiers.HasFlag(AccessModifiers.Public);
	//	}

	//	return false;
	//}

	/// <summary>
	///     Checks if the <paramref name="propertyInfo" /> has an attribute which satisfies the <paramref name="predicate" />.
	/// </summary>
	/// <typeparam name="TAttribute">The type of the <see cref="Attribute" />.</typeparam>
	/// <param name="propertyInfo">The <see cref="PropertyInfo" /> which is checked to have the attribute.</param>
	/// <param name="predicate">
	///     (optional) A predicate to check the attribute values.
	///     <para />
	///     If not set (<see langword="null" />), will only check if the attribute is present.
	/// </param>
	/// <param name="inherit">
	///     <see langword="true" /> to search the inheritance chain to find the attributes; otherwise,
	///     <see langword="false" />.<br />
	///     Defaults to <see langword="true" />
	/// </param>
	public static bool HasAttribute<TAttribute>(
		this PropertyInfo propertyInfo,
		Func<TAttribute, bool>? predicate = null,
		bool inherit = true)
		where TAttribute : Attribute
	{
		object? attribute = Attribute.GetCustomAttributes(propertyInfo, typeof(TAttribute), inherit)
			.FirstOrDefault();
		if (attribute is TAttribute castedAttribute)
		{
			return predicate?.Invoke(castedAttribute) ?? true;
		}

		return false;
	}
}
