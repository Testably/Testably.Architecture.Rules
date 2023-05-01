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
	///     (optional) A predicate to check.
	///     <para />
	///     If not set (<see langword="null" />), will only check if the attribute is present.
	/// </param>
	public static bool HasAttribute<TAttribute>(
		this Type type,
		Func<TAttribute, bool>? predicate = null)
	{
		object? attribute = type.GetCustomAttributes(typeof(TAttribute), true)
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
	///     (optional) A predicate to check.
	///     <para />
	///     If not set (<see langword="null" />), will only check if the attribute is present on any member.
	/// </param>
	public static bool HasMethodWithAttribute<TAttribute>(
		this Type type,
		Func<TAttribute, MethodInfo, bool>? predicate = null)
	{
		predicate ??= (_, _) => true;
		return type.GetMethods().Any(
			method => method.HasAttribute<TAttribute>(a => predicate(a, method)));
	}
}
