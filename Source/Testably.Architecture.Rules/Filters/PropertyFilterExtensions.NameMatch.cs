using System.Reflection;

namespace Testably.Architecture.Rules;

public static partial class PropertyFilterExtensions
{
	/// <summary>
	///     Filter <see cref="PropertyInfo" />s where the <see cref="MemberInfo.Name" /> matches the given
	///     <paramref name="pattern" />.
	/// </summary>
	public static IPropertyFilterResult WhichNameMatches(
		this IPropertyFilter @this,
		Match pattern,
		bool ignoreCase = false)
	{
		return @this.Which(property => pattern.Matches(property.Name, ignoreCase));
	}
}
