using System;
using System.Linq;
using System.Reflection;

namespace Testably.Architecture.Testing;

/// <summary>
///     Extension methods for <see cref="MethodInfo" />.
/// </summary>
public static partial class Extensions
{
	/// <summary>
	///     Checks if the <paramref name="methodInfo" /> has an attribute which satisfies the <paramref name="predicate" />.
	/// </summary>
	/// <typeparam name="TAttribute">The type of the <see cref="Attribute" />.</typeparam>
	/// <param name="methodInfo">The <see cref="MethodInfo" /> which is checked to have the attribute.</param>
	/// <param name="predicate">
	///     (optional) A predicate to check.
	///     <para />
	///     If not set (<see langword="null" />), will only check if the attribute is present.
	/// </param>
	public static bool HasAttribute<TAttribute>(
		this MethodInfo methodInfo,
		Func<TAttribute, bool>? predicate = null)
	{
		object? attribute = methodInfo.GetCustomAttributes(typeof(TAttribute), true)
		   .FirstOrDefault();
		if (attribute is TAttribute castedAttribute)
		{
			return predicate?.Invoke(castedAttribute) ?? true;
		}

		return false;
	}
}
