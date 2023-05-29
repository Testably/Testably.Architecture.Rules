using System;
using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     Define rules about the architectural design.
/// </summary>
public interface IExpectation
{
	/// <summary>
	///     Define rules for <see cref="Assembly" />s.
	/// </summary>
	IAssemblyExpectation Assemblies { get; }

	/// <summary>
	///     Define rules for <see cref="ConstructorInfo" />s.
	/// </summary>
	IConstructorExpectation Constructors { get; }

	/// <summary>
	///     Define rules for <see cref="EventInfo" />s.
	/// </summary>
	IEventExpectation Events { get; }

	/// <summary>
	///     Define rules for <see cref="FieldInfo" />s.
	/// </summary>
	IFieldExpectation Fields { get; }

	/// <summary>
	///     Define rules for <see cref="MethodInfo" />s.
	/// </summary>
	IMethodExpectation Methods { get; }

	/// <summary>
	///     Define rules for <see cref="PropertyInfo" />s.
	/// </summary>
	IPropertyExpectation Properties { get; }

	/// <summary>
	///     Define rules for <see cref="Type" />s.
	/// </summary>
	ITypeExpectation Types { get; }
}
