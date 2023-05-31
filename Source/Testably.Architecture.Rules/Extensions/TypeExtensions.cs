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
	///     Searches for constructors in the <paramref name="type" /> that were directly declared there.
	/// </summary>
	public static ConstructorInfo[] GetDeclaredConstructors(
		this Type type)
	{
		return type
			.GetConstructors(BindingFlags.DeclaredOnly |
			                 BindingFlags.NonPublic |
			                 BindingFlags.Public |
			                 BindingFlags.Instance)
			.ToArray();
	}

	/// <summary>
	///     Searches for fields in the <paramref name="type" /> that were directly declared there.
	/// </summary>
	public static FieldInfo[] GetDeclaredFields(
		this Type type)
	{
		return type
			.GetFields(BindingFlags.DeclaredOnly |
			           BindingFlags.NonPublic |
			           BindingFlags.Public |
			           BindingFlags.Instance)
			.Where(m => !m.IsSpecialName)
			.Where(m => !m.Name.EndsWith("__BackingField"))
			.ToArray();
	}

	/// <summary>
	///     Searches for methods in the <paramref name="type" /> that were directly declared there.
	/// </summary>
	public static MethodInfo[] GetDeclaredMethods(
		this Type type)
	{
		return type
			.GetMethods(BindingFlags.DeclaredOnly |
			            BindingFlags.NonPublic |
			            BindingFlags.Public |
			            BindingFlags.Static |
			            BindingFlags.Instance)
			.Where(m => !m.IsSpecialName)
			.ToArray();
	}

	/// <summary>
	///     Checks if the <paramref name="type" /> has the specified <paramref name="accessModifiers" />.
	/// </summary>
	/// <param name="type">The <see cref="MethodInfo" /> which is checked to have the attribute.</param>
	/// <param name="accessModifiers">
	///     The <see cref="AccessModifiers" />.
	///     <para />
	///     Supports specifying multiple <see cref="AccessModifiers" />.
	/// </param>
	public static bool HasAccessModifier(
		this Type type,
		AccessModifiers accessModifiers)
	{
		if (type.IsNested)
		{
			return HasAccessModifierForNestedClass(type, accessModifiers);
		}

		return HasAccessModifierForNotNestedClass(type, accessModifiers);
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
		if (attribute is TAttribute attributeValue)
		{
			return predicate?.Invoke(attributeValue) ?? true;
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

		Type currentType = type;

		int level = 0;
		while (currentType != typeof(object))
		{
			if (parentType.IsEqualTo(currentType))
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

			currentType = currentType.BaseType;
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
	///     <paramref name="parentType" />.
	/// </summary>
	/// <param name="type">The <see cref="Type" />.</param>
	/// <param name="parentType">The parent <see cref="Type" />.</param>
	/// <param name="forceDirect">
	///     If set to <see langword="false" /> (default value), the <paramref name="parentType" />
	///     can be anywhere in the inheritance tree, otherwise if set to <see langword="true" /> requires the
	///     <paramref name="parentType" /> to be the direct parent.
	/// </param>
	public static bool IsOrInheritsFrom(
		this Type type,
		Type parentType,
		bool forceDirect = false)
	{
		if (type.IsEqualTo(parentType))
		{
			return true;
		}

		return !forceDirect && type.InheritsFrom(parentType);
	}

	/// <summary>
	///     Gets a value indicating whether the <see cref="Type" /> is static.
	/// </summary>
	/// <param name="type">The <see cref="Type" />.</param>
	/// <remarks>https://stackoverflow.com/a/1175901</remarks>
	public static bool IsStatic(this Type type)
		=> type is { IsAbstract: true, IsSealed: true, IsInterface: false };

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
			Type[] typeArguments = type.GetGenericArguments();
			Type[] otherArguments = other.GetGenericArguments();
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

	private static bool HasAccessModifierForNestedClass(Type type, AccessModifiers accessModifiers)
	{
		if (accessModifiers.HasFlag(AccessModifiers.Internal) &&
		    type.IsNestedAssembly)
		{
			return true;
		}

		if (accessModifiers.HasFlag(AccessModifiers.Protected) &&
		    type.IsNestedFamily)
		{
			return true;
		}

		if (accessModifiers.HasFlag(AccessModifiers.Private) &&
		    type.IsNestedPrivate)
		{
			return true;
		}

		if (accessModifiers.HasFlag(AccessModifiers.Public) &&
		    type.IsNestedPublic)
		{
			return true;
		}

		return false;
	}

	private static bool HasAccessModifierForNotNestedClass(Type type,
		AccessModifiers accessModifiers)
	{
		if (accessModifiers.HasFlag(AccessModifiers.Internal) &&
		    !type.IsVisible)
		{
			return true;
		}

		if (accessModifiers.HasFlag(AccessModifiers.Public) &&
		    type.IsPublic)
		{
			return true;
		}

		return accessModifiers.HasFlag(AccessModifiers.Private) &&
		       type.IsNotPublic;
	}
}
