using System;
using System.Reflection;

namespace Testably.Architecture.Rules;

public static partial class FieldFilterExtensions
{
	/// <summary>
	///     Filter <see cref="FieldInfo" />s where the <see cref="MemberInfo.Name" /> matches the given <paramref name="pattern" />.
	/// </summary>
	public static IFieldFilterResult WithName(
		this IFieldFilter @this,
		Match pattern,
		bool ignoreCase = false)
	{
		return @this.Which(field => pattern.Matches(field.Name, ignoreCase));
	}
}
