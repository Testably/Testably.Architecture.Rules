using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     Add additional filters on the <see cref="Assembly" />s.
/// </summary>
public interface IAssemblyFilterResult : IRequirement<Assembly>
{
	/// <summary>
	///     Add additional filters on the <see cref="Assembly" />s.
	/// </summary>
	IAssemblyFilter And { get; }

	/// <summary>
	///     Get all types from the filtered assemblies.
	/// </summary>
	ITypeExpectation Types { get; }
}
