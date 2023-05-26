using System;
using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     Add additional filters on the <see cref="MethodInfo" />s.
/// </summary>
public interface IMethodFilterResult : IFilter<MethodInfo>, IRequirement<MethodInfo>
{
	/// <summary>
	///     Add additional filters on the <see cref="MethodInfo" />s.
	/// </summary>
	IMethodFilter And { get; }

	/// <summary>
	///     Get all types from the filtered methods.
	/// </summary>
	ITypeExpectation Types { get; }
}
