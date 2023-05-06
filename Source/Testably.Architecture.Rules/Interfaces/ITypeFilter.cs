using System;

namespace Testably.Architecture.Rules;

/// <summary>
///     Defines expectations on <see cref="Type" /> that can be filtered.
/// </summary>
public interface ITypeFilter
{
	/// <summary>
	///     Filters the applicable <see cref="Type" /> on which the expectations should be applied.
	/// </summary>
	/// <param name="filter">The filter to apply on the <see cref="Type" />.</param>
	ITypeFilterResult Which(Filter<Type> filter);
}
