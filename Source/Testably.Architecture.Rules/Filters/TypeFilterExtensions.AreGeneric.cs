namespace Testably.Architecture.Rules;

public static partial class TypeFilterExtensions
{
	/// <summary>
	///     Filter for generic types.
	/// </summary>
	public static ITypeFilterResult WhichAreGeneric(this ITypeFilter @this)
	{
		return @this.Which(
			type => type.IsGenericType,
			"is generic");
	}

	/// <summary>
	///     Filter for not generic types.
	/// </summary>
	public static ITypeFilterResult WhichAreNotGeneric(this ITypeFilter @this)
	{
		return @this.Which(
			type => !type.IsGenericType,
			"is not generic");
	}
}
