using System;
using System.Reflection;

namespace Testably.Architecture.Testing;

/// <summary>
///     Definition for expectations on the architectural design.
/// </summary>
public interface IExpectation
{
	/// <summary>
	///     Defines expectations on all given <paramref name="assemblies" />.
	/// </summary>
	IFilterableAssemblyExpectation Assembly(params Assembly[] assemblies);

	/// <summary>
	///     Defines expectations on all given <paramref name="types" />.
	/// </summary>
	IFilterableTypeExpectation Type(params Type[] types);
}
