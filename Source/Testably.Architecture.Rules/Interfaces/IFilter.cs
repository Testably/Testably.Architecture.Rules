namespace Testably.Architecture.Rules;

/// <summary>
///     Defines filter on <typeparamref name="TType" />.
/// </summary>
public interface IFilter<in TType>
{
	/// <summary>
	///     Applies the filter on the <paramref name="type" />.
	/// </summary>
	bool Applies(TType type);
}
