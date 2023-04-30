using System;

namespace Testably.Architecture.Testing;

/// <summary>
///     Defines expectations on <see cref="Type" />s that can be filtered.
/// </summary>
public interface IFilterableTypeExpectation : ITypeExpectation
{
	/// <summary>
	///     Filters the applicable <see cref="Type" /> on which the expectations should be applied.
	/// </summary>
	/// <param name="predicate">The predicate which the <see cref="Type" /> must fulfill.</param>
	IFilterableTypeExpectation Which(Func<Type, bool> predicate);
}
