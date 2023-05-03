namespace Testably.Architecture.Testing;

/// <summary>
///     Add additional filters on the <typeparamref name="TType" />s.
/// </summary>
public interface IExpectationFilterResult<TType> : IExpectationCondition<TType>
{
	/// <summary>
	///     Add additional filters on the <typeparamref name="TType" />s.
	/// </summary>
	IExpectationFilter<TType> And { get; }
}
