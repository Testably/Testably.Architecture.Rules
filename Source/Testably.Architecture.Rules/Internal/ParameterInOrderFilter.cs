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
			item => ApplyFilter(item.Key, item.Value, parameterInfos));
	}

	/// <inheritdoc cref="IOrderedParameterFilterResult.At(int)" />
	public IParameterFilter<IOrderedParameterFilterResult> At(int position)
	{
		_currentIndex = position;
		return this;
	}

	/// <inheritdoc cref="IOrderedParameterFilterResult.Then()" />
	public IParameterFilter<IOrderedParameterFilterResult> Then()
	{
		if (_currentIndex < 0)
		{
			_currentIndex--;
		}
		else
		{
			_currentIndex++;
		}

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

	private static bool ApplyFilter(int index, List<Filter<ParameterInfo>> filters,
		ParameterInfo[] parameterInfos)
	{
		if (index < 0)
		{
			index = parameterInfos.Length + index;
		}

		return index >= 0 && index < parameterInfos.Length &&
		       filters.All(f => f.Applies(parameterInfos[index]));
	}

	private static string IndexToString(int index)
	{
		if (index < 0)
		{
			return index == -1
				? "last"
				: $"{IndexToString(-1 * (index + 1))} to last";
		}

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
