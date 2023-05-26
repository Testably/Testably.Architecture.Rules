using System;
using System.Reflection;

namespace Testably.Architecture.Rules;

public static partial class MethodFilterExtensions
{
	/// <summary>
	///     Filters for <see cref="MethodInfo" />s that satisfy the <paramref name="parameterFilter" />.
	/// </summary>
	/// <param name="this">The <see cref="IMethodFilter" />.</param>
	/// <param name="parameterFilter">The filter to apply on the <see cref="MethodBase.GetParameters()" />.</param>
	public static IMethodFilterResult With(
		this IMethodFilter @this,
		IOrderedParameterFilterResult parameterFilter)
	{
		return @this.Which(Filter.FromPredicate<MethodInfo>(
			methodInfo => parameterFilter.Apply(methodInfo.GetParameters()),
			parameterFilter.FriendlyName()));
	}

	/// <summary>
	///     Filters for <see cref="MethodInfo" />s that satisfy the <paramref name="parameterFilter" />.
	/// </summary>
	/// <param name="this">The <see cref="IMethodFilter" />.</param>
	/// <param name="parameterFilter">The filter to apply on the <see cref="MethodBase.GetParameters()" />.</param>
	public static IMethodFilterResult With(
		this IMethodFilter @this,
		IUnorderedParameterFilterResult parameterFilter)
	{
		return @this.Which(Filter.FromPredicate<MethodInfo>(
			methodInfo => parameterFilter.Apply(methodInfo.GetParameters()),
			parameterFilter.FriendlyName()));
	}

	/// <summary>
	///     Filters for <see cref="MethodInfo" />s with no parameters.
	/// </summary>
	public static IMethodFilterResult WithoutParameter(
		this IMethodFilter @this)
	{
		return @this.Which(method => method.GetParameters().Length == 0);
	}
}
