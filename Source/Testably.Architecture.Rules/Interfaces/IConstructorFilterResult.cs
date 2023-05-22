using System;
using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     Add additional filters on the <see cref="ConstructorInfo" />s.
/// </summary>
public interface IConstructorFilterResult
{
	/// <summary>
	///     Add additional filters on the <see cref="ConstructorInfo" />s.
	/// </summary>
	IConstructorFilter And { get; }

	/// <summary>
	///     Create a <see cref="Filter{Type}" /> which satisfies all constructor filters.
	/// </summary>
	Filter<Type> ToTypeFilter();
}