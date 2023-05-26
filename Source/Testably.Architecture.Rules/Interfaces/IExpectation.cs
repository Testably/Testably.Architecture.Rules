using System;
using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     Definition for expectations on the architectural design.
/// </summary>
public interface IExpectation
{
	/// <summary>
	///     Defines expectations for <see cref="Assembly" />.
	/// </summary>
	IAssemblyExpectation Assemblies { get; }

	/// <summary>
	///     Defines expectations for <see cref="ConstructorInfo" />.
	/// </summary>
	IConstructorExpectation Constructors { get; }

	/// <summary>
	///     Defines expectations for <see cref="EventInfo" />.
	/// </summary>
	IEventExpectation Events { get; }

	/// <summary>
	///     Defines expectations for <see cref="FieldInfo" />.
	/// </summary>
	IFieldExpectation Fields { get; }

	/// <summary>
	///     Defines expectations for <see cref="MethodInfo" />.
	/// </summary>
	IMethodExpectation Methods { get; }

	/// <summary>
	///     Defines expectations for <see cref="PropertyInfo" />.
	/// </summary>
	IPropertyExpectation Properties { get; }

	/// <summary>
	///     Defines expectations for <see cref="Type" />.
	/// </summary>
	ITypeExpectation Types { get; }
}
