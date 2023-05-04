namespace Testably.Architecture.Testing;

/// <summary>
///     Add additional filters on the <typeparamref name="TType" />s.
/// </summary>
public interface IFilterResult<TType> : IRequirement<TType>
{
	/// <summary>
	///     Add additional filters on the <typeparamref name="TType" />s.
	/// </summary>
	IFilter<TType> And { get; }
}
