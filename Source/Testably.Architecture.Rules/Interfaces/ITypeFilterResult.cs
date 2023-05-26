using System;

namespace Testably.Architecture.Rules;

/// <summary>
///     Add additional filters on the <see cref="Type" />s.
/// </summary>
public interface ITypeFilterResult : IRequirement<Type>
{
	/// <summary>
	///     Add additional filters on the <see cref="Type" />s.
	/// </summary>
	ITypeFilter And { get; }

	/// <summary>
	///     Get all assemblies from the filtered types.
	/// </summary>
	IAssemblyExpectation Assemblies { get; }

	/// <summary>
	///     Get all constructors from the filtered types.
	/// </summary>
	IConstructorExpectation Constructors { get; }

	/// <summary>
	///     Get all events from the filtered types.
	/// </summary>
	IEventExpectation Events { get; }

	/// <summary>
	///     Get all fields from the filtered types.
	/// </summary>
	IFieldExpectation Fields { get; }

	/// <summary>
	///     Get all methods from the filtered types.
	/// </summary>
	IMethodExpectation Methods { get; }

	/// <summary>
	///     Get all properties from the filtered types.
	/// </summary>
	IPropertyExpectation Properties { get; }
}
