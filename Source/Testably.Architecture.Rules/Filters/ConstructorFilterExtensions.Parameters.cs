using System.Reflection;

namespace Testably.Architecture.Rules;

public static partial class ConstructorFilterExtensions
{
	/// <summary>
	///     Filters for <see cref="ConstructorInfo" />s that satisfy the <paramref name="parameterFilter" />.
	/// </summary>
	/// <param name="this">The <see cref="IConstructorFilter" />.</param>
	/// <param name="parameterFilter">The filter to apply on the <see cref="MethodBase.GetParameters()" />.</param>
	public static IConstructorFilterResult With(
		this IConstructorFilter @this,
		IOrderedParameterFilterResult parameterFilter)
	{
		return @this.Which(Filter.FromPredicate<ConstructorInfo>(
			constructorInfo => parameterFilter.Apply(constructorInfo.GetParameters()),
			parameterFilter.FriendlyName()));
	}

	/// <summary>
	///     Filters for <see cref="ConstructorInfo" />s that satisfy the <paramref name="parameterFilter" />.
	/// </summary>
	/// <param name="this">The <see cref="IConstructorFilter" />.</param>
	/// <param name="parameterFilter">The filter to apply on the <see cref="MethodBase.GetParameters()" />.</param>
	public static IConstructorFilterResult With(
		this IConstructorFilter @this,
		IUnorderedParameterFilterResult parameterFilter)
	{
		return @this.Which(Filter.FromPredicate<ConstructorInfo>(
			constructorInfo => parameterFilter.Apply(constructorInfo.GetParameters()),
			parameterFilter.FriendlyName()));
	}

	/// <summary>
	///     Filters for <see cref="ConstructorInfo" />s with no parameters.
	/// </summary>
	public static IConstructorFilterResult WithoutParameter(
		this IConstructorFilter @this)
	{
		return @this.Which(
			constructor => constructor.GetParameters().Length == 0,
			"without parameter");
	}

	/// <summary>
	///     Filters for <see cref="ConstructorInfo" />s with (at least) <paramref name="minimumCount" /> parameters.
	/// </summary>
	public static IConstructorFilterResult WithParameters(
		this IConstructorFilter @this,
		int minimumCount = 1)
	{
		return @this.Which(
			method => method.GetParameters().Length >= minimumCount,
			$"with at least {minimumCount} {(minimumCount > 1 ? "parameters" : "parameter")}");
	}
}
