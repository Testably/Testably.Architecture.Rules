using System.Reflection;

namespace Testably.Architecture.Testing;

/// <summary>
///     Defines expectations on <see cref="Assembly" />s that can be filtered.
/// </summary>
public interface IAssemblyExpectation : IExpectationStart<Assembly>
{
	/// <summary>
	///     Get all types from the filtered assemblies.
	/// </summary>
	ITypeExpectation Types { get; }
}
