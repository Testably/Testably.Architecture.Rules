using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Testably.Architecture.Rules.Internal;

internal class ParameterInOrderFilter : IParameterFilter<IOrderedParameterFilterResult>,
	IOrderedParameterFilterResult
{
	private int _currentIndex;
	private readonly Dictionary<int, List<Filter<ParameterInfo>>> _filters = new();

	#region IOrderedParameterFilterResult Members

	/// <inheritdoc cref="IParameterFilterResult{IOrderedParameterFilterResult}.And" />
	public IParameterFilter<IOrderedParameterFilterResult> And => this;

	/// <inheritdoc cref="IOrderedParameterFilterResult.Apply(ParameterInfo[])" />
	public bool Apply(ParameterInfo[] parameterInfos)
	{
		return _filters.All(
			item => parameterInfos.Length > item.Key &&
			        item.Value.All(f => f.Applies(parameterInfos[item.Key])));
	}

	/// <inheritdoc cref="IOrderedParameterFilterResult.Then" />
	public IParameterFilter<IOrderedParameterFilterResult> Then()
	{
		_currentIndex++;
		return this;
	}

	/// <inheritdoc />
	public string FriendlyName()
	{
		return string.Join(" and ",
			_filters.Select(item
				=> $"{IndexToString(item.Key)} parameter {string.Join(" and ", item.Value)}"));
	}

	#endregion

	#region IParameterFilter<IOrderedParameterFilterResult> Members

	/// <inheritdoc cref="IParameterFilter{IOrderedParameterFilterResult}.Which(Filter{ParameterInfo})" />
	public IOrderedParameterFilterResult Which(Filter<ParameterInfo> filter)
	{
		if (!_filters.ContainsKey(_currentIndex))
		{
			_filters.Add(_currentIndex, new List<Filter<ParameterInfo>>());
		}

		_filters[_currentIndex].Add(filter);
		return this;
	}

	#endregion

	/// <inheritdoc cref="object.ToString()" />
	public override string ToString()
		=> FriendlyName();

	private static string IndexToString(int index)
	{
		if (index == 0)
		{
			return "1st";
		}

		if (index == 1)
		{
			return "2nd";
		}

		if (index == 2)
		{
			return "3rd";
		}

		return $"{index + 1}th";
	}
}
