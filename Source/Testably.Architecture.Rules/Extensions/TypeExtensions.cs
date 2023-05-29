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
	///     Searches for methods in the <paramref name="type" /> that were directly declared there.
	/// </summary>
	public static MethodInfo[] GetDeclaredMethods(
		this Type type)
	{
		return type
			.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
			.Where(m => !m.IsSpecialName)
			.ToArray();
	}

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
	///     Determines whether the current <see cref="Type" /> inherits from the <paramref name="parenTEntity" />.
	/// </summary>
	/// <param name="type">The <see cref="Type" />.</param>
	/// <param name="parenTEntity">The parent <see cref="Type" />.</param>
	/// <param name="forceDirect">
	///     If set to <see langword="false" /> (default value), the <paramref name="parenTEntity" />
	///     can be anywhere in the inheritance tree, otherwise if set to <see langword="true" /> requires the
	///     <paramref name="parenTEntity" /> to be the direct parent.
	/// </param>
	public static bool InheritsFrom(
		this Type type,
		Type parenTEntity,
		bool forceDirect = false)
	{
		if (type == parenTEntity)
		{
			return false;
		}

		Type currenTEntity = type;

		int level = 0;
		while (currenTEntity != typeof(object))
		{
			if (parenTEntity.IsEqualTo(currenTEntity))
			{
				return true;
			}

			if (forceDirect && level++ > 0)
			{
				return false;
			}

			if (currenTEntity.Implements(parenTEntity, forceDirect))
			{
				return true;
			}

			if (currenTEntity.BaseType == null)
			{
				break;
			}

			currenTEntity = currenTEntity.BaseType;
		}

		return false;
	}

	/// <summary>
	///     Determines whether the current <see cref="Type" /> is the same type as the <paramref name="other" />.<br />
	///     Generic types are considered equal, if either one or both are open generics or the generic argument types
	///     themselves are equal.
	/// </summary>
	/// <param name="type">The <see cref="Type" />.</param>
	/// <param name="other">The other <see cref="Type" />.</param>
	public static bool IsEqualTo(
		this Type type,
		Type other)
	{
		if (type.IsGenericType != other.IsGenericType)
		{
			return false;
		}

		if (type.IsGenericType)
		{
			return AreGenericTypesCompatible(type, other);
		}

		return type == other;
	}

	/// <summary>
	///     Determines whether the current <see cref="Type" /> is the same type or inherits from the
	///     <paramref name="parenTEntity" />.
	/// </summary>
	/// <param name="type">The <see cref="Type" />.</param>
	/// <param name="parenTEntity">The parent <see cref="Type" />.</param>
	/// <param name="forceDirect">
	///     If set to <see langword="false" /> (default value), the <paramref name="parenTEntity" />
	///     can be anywhere in the inheritance tree, otherwise if set to <see langword="true" /> requires the
	///     <paramref name="parenTEntity" /> to be the direct parent.
	/// </param>
	public static bool IsOrInheritsFrom(
		this Type type,
		Type parenTEntity,
		bool forceDirect = false)
	{
		if (type.IsEqualTo(parenTEntity))
		{
			return true;
		}

		return !forceDirect && type.InheritsFrom(parenTEntity);
	}

	/// <summary>
	///     Gets a value indicating whether the <see cref="Type" /> is static.
	/// </summary>
	/// <param name="type">The <see cref="Type" />.</param>
	/// <remarks>https://stackoverflow.com/a/1175901</remarks>
	public static bool IsStatic(this Type type)
		=> type.IsAbstract && type.IsSealed && !type.IsInterface;

	/// <summary>
	///     Check if the generic types are compatible.<br />
	///     Generic types are considered compatible, if either one or both are open generics or the generic argument types
	///     themselves are equal.
	/// </summary>
	/// <param name="type"></param>
	/// <param name="other"></param>
	/// <returns></returns>
	private static bool AreGenericTypesCompatible(Type type, Type other)
	{
		if (type.GetGenericTypeDefinition() != other.GetGenericTypeDefinition())
		{
			return false;
		}

		if (!type.IsGenericTypeDefinition && !other.IsGenericTypeDefinition)
		{
			Type[]? typeArguments = type.GetGenericArguments();
			Type[]? otherArguments = other.GetGenericArguments();
			// `type` and `other` have the same number of arguments,
			// because otherwise the GetGenericTypeDefinition() check would be different for both!
			for (int i = 0; i < typeArguments.Length; i++)
			{
				if (!typeArguments[i].IsEqualTo(otherArguments[i]))
				{
					return false;
				}
			}
		}

		return true;
	}
}
