using System;
using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     Add additional filters on the <see cref="MethodInfo" />s.
/// </summary>
public interface IMethodFilterResult
{
	/// <summary>
	///     Add additional filters on the <see cref="MethodInfo" />s.
	/// </summary>
	IMethodFilter And { get; }

	/// <summary>
	///     Create a <see cref="Filter{Type}" /> which satisfies all method filters.
	/// </summary>
	Filter<Type> ToTypeFilter();
}
