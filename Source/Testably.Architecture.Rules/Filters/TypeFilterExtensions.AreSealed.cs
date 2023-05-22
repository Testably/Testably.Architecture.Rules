namespace Testably.Architecture.Rules;

public static partial class TypeFilterExtensions
{
	/// <summary>
	///     Filter for unsealed types.
	/// </summary>
	public static ITypeFilterResult WhichAreNotSealed(this ITypeFilter @this)
	{
		return @this.Which(type => !type.IsSealed);
	}

	/// <summary>
	///     Filter for sealed types.
	/// </summary>
	public static ITypeFilterResult WhichAreSealed(this ITypeFilter @this)
	{
		return @this.Which(type => type.IsSealed);
	}
}
