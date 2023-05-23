using System;
using System.Linq;
using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     Extension methods for <see cref="Type" />.
/// </summary>
public static class TypeExtensions
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

	/// <summary>
	///     Determines whether the current <see cref="Type" /> implements the <paramref name="interfaceType" />.
	/// </summary>
	/// <param name="type">The <see cref="Type" />.</param>
	/// <param name="interfaceType">The interface <see cref="Type" />.</param>
	/// <param name="forceDirect">
	///     If set to <see langword="false" /> (default value), the <paramref name="interfaceType" />
	///     can be anywhere in the inheritance tree, otherwise if set to <see langword="true" /> requires the
	///     <paramref name="interfaceType" /> to be directly implemented in the <paramref name="type" />.
	/// </param>
	public static bool Implements(
		this Type type,
		Type interfaceType,
		bool forceDirect = false)
	{
		if (!interfaceType.IsInterface)
		{
			return false;
		}

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
	public static bool InheritsFrom(
		this Type type,
		Type parentType,
		bool forceDirect = false)
	{
		if (type == parentType)
		{
			return false;
		}

		Type currentType = type.IsGenericType
			? type.GetGenericTypeDefinition()
			: type;

		int level = 0;
		while (currentType != typeof(object))
		{
			if (parentType == currentType)
			{
				return true;
			}

			if (forceDirect && level++ > 0)
			{
				return false;
			}

			if (currentType.Implements(parentType, forceDirect))
			{
				return true;
			}

			if (currentType.BaseType == null)
			{
				break;
			}

			currentType = currentType.BaseType.IsGenericType
				? currentType.BaseType.GetGenericTypeDefinition()
				: currentType.BaseType;
		}

		return false;
	}

	/// <summary>
	///     Gets a value indicating whether the <see cref="Type" /> is static.
	/// </summary>
	/// <param name="type">The <see cref="Type" />.</param>
	/// <remarks>https://stackoverflow.com/a/1175901</remarks>
	public static bool IsStatic(this Type type)
		=> type.IsAbstract && type.IsSealed && !type.IsInterface &&
		   !type.GetConstructors().Any(m => m.IsPublic);
}
