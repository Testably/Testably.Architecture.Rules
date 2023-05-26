using System;
using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     Add additional filters on the <see cref="ConstructorInfo" />s.
/// </summary>
public interface IConstructorFilterResult : IFilter<ConstructorInfo>, IRequirement<ConstructorInfo>
{
	/// <summary>
	///     Add additional filters on the <see cref="ConstructorInfo" />s.
	/// </summary>
	IConstructorFilter And { get; }

	/// <summary>
	///     Get all types from the filtered constructors.
	/// </summary>
	ITypeExpectation Types { get; }
}
