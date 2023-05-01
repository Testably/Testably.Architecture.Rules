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
}
