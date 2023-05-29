using System.Collections.Generic;

namespace Testably.Architecture.Rules.Internal;

internal abstract class Rule<TEntity> :
	IRequirement<TEntity>, IRequirementResult<TEntity>, IExemptionResult<TEntity>
{
	protected List<Exemption> Exemptions { get; } = new();
	protected List<Filter<TEntity>> Filters { get; } = new();
	protected List<Requirement<TEntity>> Requirements { get; } = new();

	#region IExemptionResult<TEntity> Members

	/// <inheritdoc cref="IExemptionResult{TEntity}.And" />
	IExemption<TEntity> IExemptionResult<TEntity>.And => this;

	#endregion

	#region IRequirement<TEntity> Members

	/// <inheritdoc cref="IRequirement{TEntity}.ShouldSatisfy(Requirement{TEntity})" />
	public IRequirementResult<TEntity> ShouldSatisfy(Requirement<TEntity> requirement)
	{
		Requirements.Add(requirement);
		return this;
	}

	#endregion

	#region IRequirementResult<TEntity> Members

	/// <inheritdoc cref="IRule.Check" />
	public abstract IRuleCheck Check { get; }

	/// <inheritdoc cref="IRequirementResult{TEntity}.And" />
	IRequirement<TEntity> IRequirementResult<TEntity>.And => this;

	/// <inheritdoc cref="IExemption{TEntity}.Unless(Exemption)" />
	public IExemptionResult<TEntity> Unless(Exemption exemption)
	{
		Exemptions.Add(exemption);
		return this;
	}

	#endregion
}
