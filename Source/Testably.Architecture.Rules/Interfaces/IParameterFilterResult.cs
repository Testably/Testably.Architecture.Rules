using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     Add additional filters on the array of <see cref="ParameterInfo" />s.
/// </summary>
public interface IParameterFilterResult<TSelf>
	where TSelf : IParameterFilterResult<TSelf>
{
	/// <summary>
	///     Add additional filters on the array of <see cref="ParameterInfo" />s.
	/// </summary>
	IParameterFilter<TSelf> And { get; }
}
