using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     Add additional filters on the array of <see cref="ParameterInfo" />s.
/// </summary>
public interface IOrderedParameterFilterResult
	: IParameterFilterResult<IOrderedParameterFilterResult>
{
	/// <summary>
	///     Applies the filter on the array of <see cref="ParameterInfo" />s.
	/// </summary>
	bool Apply(ParameterInfo[] parameterInfos);

	/// <summary>
	///     Specifies an explicit position of the parameter.
	///     <para />
	///     Non-negative values are zero-based index from the start.<br />
	///     Negative values count from the last parameter back (e.g. `-1` indicates the last parameter).
	/// </summary>
	IParameterFilter<IOrderedParameterFilterResult> At(int position);

	/// <summary>
	///     Specifies filters on the next or previous <see cref="ParameterInfo" />.
	///     <para />
	///     In ascending order (when using <see cref="Parameters.First" /> the next parameter is used.<br />
	///     In descending order (when using <see cref="Parameters.Last" /> the previous parameter is used.
	/// </summary>
	IParameterFilter<IOrderedParameterFilterResult> Then();
}
