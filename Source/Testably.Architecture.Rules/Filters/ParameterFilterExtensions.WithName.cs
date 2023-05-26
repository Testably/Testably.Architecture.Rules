using System;
using System.Reflection;

namespace Testably.Architecture.Rules;

public static partial class ParameterFilterExtensions
{
	/// <summary>
	///     Filter <see cref="ParameterInfo" />s where the <see cref="MemberInfo.Name" /> matches the given <paramref name="pattern" />.
	/// </summary>
	public static TResult WithName<TResult>(
		this IParameterFilter<TResult> @this,
		Match pattern,
		bool ignoreCase = false)
	where TResult : IParameterFilterResult<TResult>
	{
		return @this.Which(
			parameter => pattern.Matches(parameter.Name, ignoreCase),
			$"name matches '{pattern}'");
	}
}
