using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     Add additional filters on the array of <see cref="ParameterInfo" />s.
/// </summary>
public interface IUnorderedParameterFilterResult
	: IParameterFilterResult<IUnorderedParameterFilterResult>
{
	/// <summary>
	///     Applies the filter on the array of <see cref="ParameterInfo" />s.
	/// </summary>
	bool Apply(ParameterInfo[] parameterInfos);
}
