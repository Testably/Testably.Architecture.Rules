namespace Testably.Architecture.Rules;

public static partial class TypeFilterExtensions
{
	/// <summary>
	///     Filter for not public types.
	/// </summary>
	public static ITypeFilterResult WhichAreNotPublic(
		this ITypeFilter @this)
	{
		return @this.Which(type => type.IsNested ? !type.IsNestedPublic : !type.IsPublic,
			"is not public");
	}

	/// <summary>
	///     Filter for public types.
	/// </summary>
	public static ITypeFilterResult WhichArePublic(this ITypeFilter @this)
	{
		return @this.Which(type => type.IsNested ? type.IsNestedPublic : type.IsPublic,
			"is public");
	}
}
