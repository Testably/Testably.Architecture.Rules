using System.Reflection;

namespace Testably.Architecture.Rules;

public static partial class TypeFilterExtensions
{
	/// <summary>
	///     Filter for types where the <see cref="MemberInfo.Name" /> matches the given <paramref name="pattern" />.
	/// </summary>
	public static ITypeFilterResult WhichMatchName(this ITypeFilter @this,
		Match pattern,
		bool ignoreCase = false)
	{
		return @this.Which(type => pattern.Matches(type.Name, ignoreCase));
	}

	/// <summary>
	///     Filter for types where the <see cref="MemberInfo.Name" /> does not match the given <paramref name="pattern" />.
	/// </summary>
	public static ITypeFilterResult WhichDoNotMatchName(this ITypeFilter @this,
		Match pattern,
		bool ignoreCase = false)
	{
		return @this.Which(type => !pattern.Matches(type.Name, ignoreCase));
	}
}
