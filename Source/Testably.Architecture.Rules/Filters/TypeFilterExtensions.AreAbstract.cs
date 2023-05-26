namespace Testably.Architecture.Rules;

public static partial class TypeFilterExtensions
{
	/// <summary>
	///     Filter for abstract types.
	/// </summary>
	public static ITypeFilterResult WhichAreAbstract(this ITypeFilter @this)
	{
		return @this.Which(
			type => type.IsAbstract,
			"is abstract");
	}

	/// <summary>
	///     Filter for not abstract types.
	/// </summary>
	public static ITypeFilterResult WhichAreNotAbstract(this ITypeFilter @this)
	{
		return @this.Which(
			type => !type.IsAbstract,
			"is not abstract");
	}
}
