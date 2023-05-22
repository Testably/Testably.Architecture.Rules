namespace Testably.Architecture.Rules;

public static partial class TypeFilterExtensions
{
	/// <summary>
	///     Filter for un-nested types.
	/// </summary>
	public static ITypeFilterResult WhichAreNotNested(this ITypeFilter @this)
	{
		return @this.Which(type => !type.IsNested);
	}

	/// <summary>
	///     Filter for nested types.
	/// </summary>
	public static ITypeFilterResult WhichAreNested(this ITypeFilter @this)
	{
		return @this.Which(type => type.IsNested);
	}
}
