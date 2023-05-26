namespace Testably.Architecture.Rules;

public static partial class TypeFilterExtensions
{
	/// <summary>
	///     Filter for not static types.
	/// </summary>
	public static ITypeFilterResult WhichAreNotStatic(this ITypeFilter @this)
	{
		return @this.Which(
			type => !type.IsStatic(),
			"is not static");
	}

	/// <summary>
	///     Filter for static types.
	/// </summary>
	public static ITypeFilterResult WhichAreStatic(this ITypeFilter @this)
	{
		return @this.Which(
			type => type.IsStatic(),
			"is static");
	}
}
