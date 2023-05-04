namespace Testably.Architecture.Testing;

/// <summary>
///     The start of the definition of an architecture rule.
/// </summary>
public interface IExpectationStart<TType> : IFilter<TType>, IRequirement<TType>
{
	/// <summary>
	///     If set allows the filters to return an empty set of matching <typeparamref name="TType" />s.
	/// </summary>
	IExpectationStart<TType> OrNone();
}
