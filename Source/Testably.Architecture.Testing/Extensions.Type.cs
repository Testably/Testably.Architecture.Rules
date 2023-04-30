using System;
using System.Linq;
using System.Reflection;

namespace Testably.Architecture.Testing;

/// <summary>
///     Extension methods for <see cref="Type" />.
/// </summary>
public static partial class Extensions
{
	public static bool HasAttribute<TAttribute>(this Type type,
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

	public static bool HasAttribute<TAttribute>(this MethodInfo methodInfo,
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

	public static bool HasMethodWithAttribute<TAttribute>(this Type type,
	                                                      Func<TAttribute, MethodInfo,
		                                                      bool>? predicate = null)
	{
		predicate ??= (_, _) => true;
		foreach (MethodInfo method in type.GetMethods())
		{
			if (method.HasAttribute<TAttribute>(a => predicate(a, method)))
			{
				return true;
			}
		}

		return false;
	}
}
