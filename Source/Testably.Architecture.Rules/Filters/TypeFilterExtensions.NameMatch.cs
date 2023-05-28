using System.Reflection;

namespace Testably.Architecture.Rules;

public static partial class TypeFilterExtensions
{
	/// <summary>
	///     Filter for types where the <see cref="MemberInfo.Name" /> does not match the given <paramref name="pattern" />.
	/// </summary>
	public static ITypeFilterResult WhichNameDoesNotMatch(this ITypeFilter @this,
		Match pattern,
		bool ignoreCase = false)
	{
		return @this.Which(type => !pattern.Matches(type.Name, ignoreCase),
			$"name does not match pattern '{pattern}'");
	}

	/// <summary>
	///     Filter for types where the <see cref="MemberInfo.Name" /> matches the given <paramref name="pattern" />.
	/// </summary>
	public static ITypeFilterResult WhichNameMatches(this ITypeFilter @this,
		Match pattern,
		bool ignoreCase = false)
	{
		return @this.Which(
			type => pattern.Matches(type.Name, ignoreCase),
			$"name matches pattern '{pattern}'");
	}
}
