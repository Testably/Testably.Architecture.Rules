namespace Testably.Architecture.Rules;

/// <summary>
///     Defines filter on <typeparamref name="TEntity" />.
/// </summary>
public interface IFilter<in TEntity>
{
	/// <summary>
	///     Applies the filter on the <paramref name="type" />.
	/// </summary>
	bool Applies(TEntity type);
}
