namespace Testably.Architecture.Rules;

/// <summary>
///     Defines requirements on <typeparamref name="TEntity" />.
/// </summary>
public interface IRequirement<TEntity>
{
	/// <summary>
	///     The <typeparamref name="TEntity" /> should satisfy the given <paramref name="requirement" />.
	/// </summary>
	IRequirementResult<TEntity> ShouldSatisfy(Requirement<TEntity> requirement);
}
