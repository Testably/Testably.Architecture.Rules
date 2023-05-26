namespace Testably.Architecture.Rules;

public static partial class TypeFilterExtensions
{
	/// <summary>
	///     Filter for types that are an interface.
	/// </summary>
	public static ITypeFilterResult WhichAreAnInterface(this ITypeFilter @this)
	{
		return @this.Which(
			type => type.IsInterface,
			"is an interface");
	}

	/// <summary>
	///     Filter for types that are no interface.
	/// </summary>
	public static ITypeFilterResult WhichAreNotAnInterface(this ITypeFilter @this)
	{
		return @this.Which(
			type => !type.IsInterface,
			"is no interface");
	}
}
