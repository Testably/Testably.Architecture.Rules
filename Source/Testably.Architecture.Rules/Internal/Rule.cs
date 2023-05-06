using System.Collections.Generic;

namespace Testably.Architecture.Rules.Internal;

internal abstract class Rule<TType> :
	IRequirement<TType>, IRequirementResult<TType>,
	IExemption<TType>, IExemptionResult<TType>
{
	protected List<Filter<TType>> Filters { get; } = new();
	protected List<Requirement<TType>> Requirements { get; } = new();
	protected List<Exemption> Exemptions { get; } = new();

	/// <inheritdoc cref="IRequirement{TType}.ShouldSatisfy(Requirement{TType})" />
	public IRequirementResult<TType> ShouldSatisfy(Requirement<TType> requirement)
	{
		Requirements.Add(requirement);
		return this;
	}

	/// <inheritdoc cref="IExemption{TType}.Unless(Exemption)" />
	public IExemptionResult<TType> Unless(Exemption exemption)
	{
		Exemptions.Add(exemption);
		return this;
	}

	/// <inheritdoc cref="IRequirementResult{TType}.And" />
	IRequirement<TType> IRequirementResult<TType>.And => this;

	/// <inheritdoc cref="IExemptionResult{TType}.And" />
	IExemption<TType> IExemptionResult<TType>.And => this;

	/// <inheritdoc cref="IRule.Check" />
	public abstract IRuleCheck Check { get; }
}
