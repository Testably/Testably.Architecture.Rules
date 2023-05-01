using System;
using System.Linq;
using System.Reflection;

namespace Testably.Architecture.Testing;

/// <summary>
///     Extension methods for <see cref="Type" />.
/// </summary>
public static class ExtensionsForType
{
	/// <summary>
	///     Checks if the <paramref name="type" /> has an attribute which satisfies the <paramref name="predicate" />.
	/// </summary>
	/// <typeparam name="TAttribute">The type of the <see cref="Attribute" />.</typeparam>
	/// <param name="type">The <see cref="Type" /> which is checked to have the attribute.</param>
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
		this Type type,
		Func<TAttribute, bool>? predicate = null,
		bool inherit = true)
		where TAttribute : Attribute
	{
		object? attribute = type.GetCustomAttributes(typeof(TAttribute), inherit)
			.FirstOrDefault();
		if (attribute is TAttribute castedAttribute)
		{
			return predicate?.Invoke(castedAttribute) ?? true;
		}

		return false;
	}

	/// <summary>
	///     Determines whether the current <see cref="Type" /> inherits from the <paramref name="parentType" />.
	/// </summary>
	/// <param name="type">The <see cref="Type" />.</param>
	/// <param name="parentType">The parent <see cref="Type" />.</param>
	/// <param name="forceDirect">
	///     If set to <see langword="false" /> (default value), the <paramref name="parentType" />
	///     can be anywhere in the inheritance tree, otherwise if set to <see langword="true" /> requires the
	///     <paramref name="parentType" /> to be the direct parent.
	/// </param>
	/// <returns></returns>
	public static bool Inherits(
		this Type type,
		Type parentType,
		bool forceDirect = false)
	{
		bool shouldUseGenericType =
			!(parentType.IsGenericType &&
			  parentType.GetGenericTypeDefinition() != parentType);

		if (parentType.IsGenericType && shouldUseGenericType)
		{
			parentType = parentType.GetGenericTypeDefinition();
		}

		Type? currentChild = type.IsGenericType
			? type.GetGenericTypeDefinition()
			: type;

		while (currentChild != typeof(object))
		{
			if (parentType == currentChild ||
			    currentChild.ImplementsInterface(parentType, forceDirect))
			{
				return true;
			}

			if (forceDirect)
			{
				return false;
			}

			currentChild = currentChild.BaseType != null
			               && currentChild.BaseType.IsGenericType
				? currentChild.BaseType.GetGenericTypeDefinition()
				: currentChild.BaseType;

			if (currentChild == null)
			{
				return false;
			}
		}

		return false;
	}

	/// <summary>
	///     Checks if any method of the <paramref name="type" /> has an attribute which satisfies the
	///     <paramref name="predicate" />.
	/// </summary>
	/// <typeparam name="TAttribute">The type of the <see cref="Attribute" />.</typeparam>
	/// <param name="type">The <see cref="Type" /> which is checked to have a member with the attribute.</param>
	/// <param name="predicate">
	///     (optional) A predicate to check the attribute values.
	///     <para />
	///     If not set (<see langword="null" />), will only check if the attribute is present on any member.
	/// </param>
	/// <param name="inherit">
	///     <see langword="true" /> to search the inheritance chain to find the attributes; otherwise,
	///     <see langword="false" />.<br />
	///     Defaults to <see langword="true" />
	/// </param>
	public static bool HasMethodWithAttribute<TAttribute>(
		this Type type,
		Func<TAttribute, MethodInfo, bool>? predicate = null,
		bool inherit = true)
		where TAttribute : Attribute
	{
		predicate ??= (_, _) => true;
		return type.GetMethods().Any(
			method => method.HasAttribute<TAttribute>(
				a => predicate(a, method), inherit));
	}

	internal static bool ImplementsInterface(this Type type, Type interfaceType, bool forceDirect)
	{
		Type[] interfaces = type.GetInterfaces();
		if (forceDirect && type.BaseType != null)
		{
			interfaces = interfaces
				.Except(type.BaseType.GetInterfaces())
				.ToArray();
		}

		return interfaces
			.Any(childInterface =>
			{
				Type currentInterface = childInterface.IsGenericType
					? childInterface.GetGenericTypeDefinition()
					: childInterface;

				return currentInterface == interfaceType;
			});
	}
}
