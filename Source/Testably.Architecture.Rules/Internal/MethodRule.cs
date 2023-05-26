using System;
using System.Linq;
using System.Reflection;

namespace Testably.Architecture.Rules.Internal;

internal class MethodRule : Rule<MethodInfo>, IMethodExpectation, IMethodFilterResult
{
	/// <inheritdoc cref="IRule.Check" />
	public override IRuleCheck Check
		=> new RuleCheck<MethodInfo>(Filters, Requirements, Exemptions,
			_ => _.SelectMany(a => a.GetTypes().SelectMany(t => t.GetMethods())));

	public MethodRule(params Filter<MethodInfo>[] filters)
	{
		Filters.AddRange(filters);
	}

	#region IMethodExpectation Members

	/// <inheritdoc cref="IMethodFilter.Which(Filter{MethodInfo})" />
	public IMethodFilterResult Which(Filter<MethodInfo> filter)
	{
		Filters.Add(filter);
		return this;
	}

	#endregion

	#region IMethodFilterResult Members

	/// <inheritdoc cref="IMethodFilterResult.And" />
	public IMethodFilter And => this;

	/// <inheritdoc />
	public Filter<Type> ToTypeFilter()
		=> throw new NotImplementedException();

	#endregion
}
