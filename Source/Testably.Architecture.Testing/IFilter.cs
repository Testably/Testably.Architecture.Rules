namespace Testably.Architecture.Testing;

/// <summary>
///     Defines expectations on <typeparamref name="TType" /> that can be filtered.
/// </summary>
public interface IFilter<TType>
{
	/// <summary>
	///     Filters the applicable <typeparamref name="TType" /> on which the expectations should be applied.
	/// </summary>
	/// <param name="filter">The filter to apply on the <typeparamref name="TType" />.</param>
	IFilterResult<TType> Which(Filter<TType> filter);
}
