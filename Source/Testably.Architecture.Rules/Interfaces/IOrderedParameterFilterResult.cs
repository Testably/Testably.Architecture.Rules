using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     Add additional filters on the array of <see cref="ParameterInfo" />s.
/// </summary>
public interface
	IOrderedParameterFilterResult : IParameterFilterResult<IOrderedParameterFilterResult>
{
	/// <summary>
	///     Applies the filter on the array of <see cref="ParameterInfo" />s.
	/// </summary>
	bool Apply(ParameterInfo[] parameterInfos);

	/// <summary>
	///     Specifies filters on the next <see cref="ParameterInfo" />.
	/// </summary>
	IParameterFilter<IOrderedParameterFilterResult> Then();
}
