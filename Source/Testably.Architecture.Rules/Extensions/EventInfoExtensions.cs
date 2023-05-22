using System;
using System.Linq;
using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     Extension events for <see cref="EventInfo" />.
/// </summary>
public static class EventInfoExtensions
{
	/// <summary>
	///     Checks if the <paramref name="eventInfo" /> has an attribute which satisfies the <paramref name="predicate" />.
	/// </summary>
	/// <typeparam name="TAttribute">The type of the <see cref="Attribute" />.</typeparam>
	/// <param name="eventInfo">The <see cref="EventInfo" /> which is checked to have the attribute.</param>
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
		this EventInfo eventInfo,
		Func<TAttribute, bool>? predicate = null,
		bool inherit = true)
		where TAttribute : Attribute
	{
		object? attribute = eventInfo.GetCustomAttributes(typeof(TAttribute), inherit)
			.FirstOrDefault();
		if (attribute is TAttribute castedAttribute)
		{
			return predicate?.Invoke(castedAttribute) ?? true;
		}

		return false;
	}
}
