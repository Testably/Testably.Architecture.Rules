namespace Testably.Architecture.Rules;

/// <summary>
///     Defines requirements on <typeparamref name="TType" />.
/// </summary>
public interface IRequirement<TType>
{
	/// <summary>
	///     The <typeparamref name="TType" /> should satisfy the given <paramref name="requirement" />.
	/// </summary>
	IRequirementResult<TType> ShouldSatisfy(Requirement<TType> requirement);
}
