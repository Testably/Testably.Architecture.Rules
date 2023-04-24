using System.Reflection;

namespace Testably.Architecture.Testing;

/// <summary>
///     Definition for expectations on the architectural design.
/// </summary>
public interface IExpectation
{
	/// <summary>
	///     Defines expectations on all loaded projects from the provided <paramref name="assemblies" />.
	/// </summary>
	IFilterableProjectExpectation FromAssembly(params Assembly[] assemblies);
}
