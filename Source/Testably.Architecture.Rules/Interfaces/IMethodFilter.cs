using System;
using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     Defines expectations on <see cref="MethodInfo" /> that can be filtered.
/// </summary>
public interface IMethodFilter
{
	/// <summary>
	///     Filters the <see cref="Type.GetMethods()" /> on which the expectations should be applied.
	/// </summary>
	/// <param name="filter">The filter to apply on the <see cref="MethodInfo" />.</param>
	IMethodFilterResult Which(Filter<MethodInfo> filter);
}