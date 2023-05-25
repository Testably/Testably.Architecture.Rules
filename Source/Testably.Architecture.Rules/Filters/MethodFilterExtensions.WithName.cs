using System;
using System.Reflection;

namespace Testably.Architecture.Rules;

public static partial class MethodFilterExtensions
{
	/// <summary>
	///     Filter <see cref="MethodInfo" />s where the <see cref="MemberInfo.Name" /> matches the given <paramref name="pattern" />.
	/// </summary>
	public static IMethodFilterResult WithName(
		this IMethodFilter @this,
		Match pattern,
		bool ignoreCase = false)
	{
		return @this.Which(method => pattern.Matches(method.Name, ignoreCase));
	}
}
