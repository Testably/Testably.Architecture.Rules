namespace Testably.Architecture.Rules;

public static partial class TypeFilterExtensions
{
	/// <summary>
	///     Filter for not private types.
	/// </summary>
	public static ITypeFilterResult WhichAreNotPrivate(
		this ITypeFilter @this)
	{
		return @this.Which(type => type.IsNested ? !type.IsNestedPrivate : !type.IsNotPublic);
	}

	/// <summary>
	///     Filter for private types.
	/// </summary>
	public static ITypeFilterResult WhichArePrivate(this ITypeFilter @this)
	{
		return @this.Which(type => type.IsNested ? type.IsNestedPrivate : type.IsNotPublic);
	}
}
