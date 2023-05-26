using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     Defines expectations on <see cref="ParameterInfo" />s that can be filtered.
/// </summary>
public interface IParameterFilter<TResult>
	where TResult : IParameterFilterResult<TResult>
{
	/// <summary>
	///     Defines the filter for the <see cref="ParameterInfo" />.
	/// </summary>
	/// <param name="filter">The filter to apply on the <see cref="ParameterInfo" />.</param>
	TResult Which(Filter<ParameterInfo> filter);
}
