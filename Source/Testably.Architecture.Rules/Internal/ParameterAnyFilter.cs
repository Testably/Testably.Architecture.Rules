using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Testably.Architecture.Rules.Internal;

internal class ParameterAnyFilter : IParameterFilter<IUnorderedParameterFilterResult>,
	IUnorderedParameterFilterResult
{
	private readonly List<Filter<ParameterInfo>> _filters = new();

	#region IParameterFilter<IUnorderedParameterFilterResult> Members

	/// <inheritdoc cref="IParameterFilter{IUnorderedParameterFilterResult}.Which(Filter{ParameterInfo})" />
	public IUnorderedParameterFilterResult Which(Filter<ParameterInfo> filter)
	{
		_filters.Add(filter);
		return this;
	}

	#endregion

	#region IUnorderedParameterFilterResult Members

	/// <inheritdoc cref="IParameterFilterResult{IUnorderedParameterFilterResult}.And" />
	public IParameterFilter<IUnorderedParameterFilterResult> And
		=> this;

	/// <inheritdoc />
	public string FriendlyName()
		=> string.Join(" and ", _filters);

	/// <inheritdoc cref="IUnorderedParameterFilterResult.Apply(ParameterInfo[])" />
	public bool Apply(ParameterInfo[] parameterInfos)
		=> _filters.All(f => parameterInfos.Any(f.Applies));

	#endregion

	/// <inheritdoc />
	public override string ToString()
		=> FriendlyName();
}
