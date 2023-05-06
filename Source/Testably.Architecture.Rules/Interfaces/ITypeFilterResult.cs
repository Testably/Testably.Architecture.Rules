using System;

namespace Testably.Architecture.Rules;

/// <summary>
///     Add additional filters on the <see cref="Type"/>s.
/// </summary>
public interface ITypeFilterResult : IRequirement<Type>
{
	/// <summary>
	///     Add additional filters on the <see cref="Type"/>s.
	/// </summary>
	ITypeFilter And { get; }

	/// <summary>
	///     Get all assemblies from the filtered types.
	/// </summary>
	IAssemblyExpectation Assemblies { get; }
}
