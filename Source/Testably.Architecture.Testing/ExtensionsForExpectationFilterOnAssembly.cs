using System;
using System.Reflection;

namespace Testably.Architecture.Testing;

/// <summary>
///     Extension methods for <see cref="IExpectationFilter{Assembly}" />.
/// </summary>
public static class ExtensionsForExpectationFilterOnAssembly
{
	/// <summary>
	///     Filters the applicable <see cref="Assembly" /> on which the expectations should be applied.
	/// </summary>
	/// <param name="this">The <see cref="IExpectationFilter{Assembly}" />.</param>
	/// <param name="filter">The filter to apply on the <see cref="Assembly" />.</param>
	public static IExpectationFilterResult<Assembly> Which(this IExpectationFilter<Assembly> @this,
		Func<Assembly, bool> filter)
	{
		return @this.Which(Filter<Assembly>.FromPredicate(filter));
	}
}
