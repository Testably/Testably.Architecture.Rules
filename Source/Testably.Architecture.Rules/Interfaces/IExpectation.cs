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
	///     Defines expectations for <see cref="MethodInfo" />.
	/// </summary>
	IMethodExpectation Methods { get; }

	/// <summary>
	///     Defines expectations for <see cref="Type" />.
	/// </summary>
	ITypeExpectation Types { get; }
}
