using System;
using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     Defines expectations on <see cref="Assembly" />s that can be filtered.
/// </summary>
public interface IAssemblyExpectation : IFilter<Assembly>, IRequirement<Assembly>
{
	/// <summary>
	///     Get all types from the filtered assemblies.
	/// </summary>
	IFilter<Type> Types { get; }
}
