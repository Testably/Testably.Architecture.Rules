using System;
using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     Add additional filters on the <see cref="PropertyInfo" />s.
/// </summary>
public interface IPropertyFilterResult
{
	/// <summary>
	///     Add additional filters on the <see cref="PropertyInfo" />s.
	/// </summary>
	IPropertyFilter And { get; }

	/// <summary>
	///     Create a <see cref="Filter{Type}" /> which satisfies all property filters.
	/// </summary>
	Filter<Type> ToTypeFilter();
}
