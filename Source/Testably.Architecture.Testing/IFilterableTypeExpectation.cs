using System;

namespace Testably.Architecture.Testing;

/// <summary>
///     Defines expectations on <see cref="Type" />s that can be filtered.
/// </summary>
public interface IFilterableTypeExpectation
{
	/// <summary>
	///     Filters the applicable <see cref="Type" /> on which the expectations should be applied.
	/// </summary>
	/// <param name="filter">The filter to apply on the <see cref="Type" />.</param>
	IFilteredTypeExpectation Which(TypeFilter filter);
}

public interface IOptionallyFilterableTypeExpectation : IFilterableTypeExpectation, ITypeExpectation
{

}
