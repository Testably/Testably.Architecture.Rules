using System;
using System.Reflection;

namespace Testably.Architecture.Testing;

/// <summary>
///     Defines expectations on <see cref="Assembly" />s that can be filtered.
/// </summary>
public interface IFilterableAssemblyExpectation : IAssemblyExpectation
{
	/// <summary>
	///     Get all types from the filtered assemblies.
	/// </summary>
	IExpectationStart<Type> Types { get; }

	/// <summary>
	///     Filters the applicable <see cref="Assembly" /> on which the expectations should be applied.
	/// </summary>
	/// <param name="predicate">The predicate which the <see cref="Assembly" /> must fulfill.</param>
	IFilterableAssemblyExpectation Which(Func<Assembly, bool> predicate);
}
