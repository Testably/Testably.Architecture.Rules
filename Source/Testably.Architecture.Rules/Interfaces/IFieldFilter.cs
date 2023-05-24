using System;
using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     Defines expectations on <see cref="FieldInfo" /> that can be filtered.
/// </summary>
public interface IFieldFilter
{
	/// <summary>
	///     Filters the <see cref="Type.GetFields()" /> on which the expectations should be applied.
	/// </summary>
	/// <param name="filter">The filter to apply on the <see cref="FieldInfo" />.</param>
	IFieldFilterResult Which(Filter<FieldInfo> filter);
}
