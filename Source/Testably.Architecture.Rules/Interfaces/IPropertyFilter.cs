using System;
using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     Defines expectations on <see cref="PropertyInfo" /> that can be filtered.
/// </summary>
public interface IPropertyFilter
{
	/// <summary>
	///     Filters the <see cref="Type.GetProperties()" /> on which the expectations should be applied.
	/// </summary>
	/// <param name="filter">The filter to apply on the <see cref="PropertyInfo" />.</param>
	IPropertyFilterResult Which(Filter<PropertyInfo> filter);
}
