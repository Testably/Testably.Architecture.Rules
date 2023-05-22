using System;
using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     Defines expectations on <see cref="ConstructorInfo" /> that can be filtered.
/// </summary>
public interface IConstructorFilter
{
	/// <summary>
	///     Filters the <see cref="Type.GetConstructors()" /> on which the expectations should be applied.
	/// </summary>
	/// <param name="filter">The filter to apply on the <see cref="ConstructorInfo" />.</param>
	IConstructorFilterResult Which(Filter<ConstructorInfo> filter);
}
