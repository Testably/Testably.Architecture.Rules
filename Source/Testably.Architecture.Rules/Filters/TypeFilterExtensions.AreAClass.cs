namespace Testably.Architecture.Rules;

public static partial class TypeFilterExtensions
{
	/// <summary>
	///     Filter for types that are a class.
	/// </summary>
	public static ITypeFilterResult WhichAreAClass(this ITypeFilter @this)
	{
		return @this.Which(type => type.IsClass);
	}

	/// <summary>
	///     Filter for types that are no class.
	/// </summary>
	public static ITypeFilterResult WhichAreNotAClass(this ITypeFilter @this)
	{
		return @this.Which(type => !type.IsClass);
	}
}
