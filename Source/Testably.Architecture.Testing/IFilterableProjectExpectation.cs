using System;
using Testably.Architecture.Testing.Models;

namespace Testably.Architecture.Testing;

/// <summary>
///     Defines expectations on project level that can be filtered.
/// </summary>
public interface IFilterableProjectExpectation : IProjectExpectation
{
	/// <summary>
	///     Filters the applicable projects on which the expectations should be applied.
	/// </summary>
	/// <param name="predicate">The predicate which the projects must fulfill.</param>
	IFilterableProjectExpectation Which(Func<Project, bool> predicate);
}
