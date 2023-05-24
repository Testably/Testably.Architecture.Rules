using System;
using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     Add additional filters on the <see cref="FieldInfo" />s.
/// </summary>
public interface IFieldFilterResult
{
	/// <summary>
	///     Add additional filters on the <see cref="FieldInfo" />s.
	/// </summary>
	IFieldFilter And { get; }

	/// <summary>
	///     Create a <see cref="Filter{Type}" /> which satisfies all field filters.
	/// </summary>
	Filter<Type> ToTypeFilter();
}
