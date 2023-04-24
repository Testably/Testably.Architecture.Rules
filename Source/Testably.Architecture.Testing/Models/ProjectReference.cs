using System.Reflection;

namespace Testably.Architecture.Testing.Models;

/// <summary>
///     Project information gathered from an <see cref="Assembly" />.
/// </summary>
public class ProjectReference
{
	/// <summary>
	///     The name of the project.
	/// </summary>
	public virtual string Name => _assemblyName.Name ?? _assemblyName.ToString();

	private readonly AssemblyName _assemblyName;

	/// <summary>
	///     Initializes a new instance of <see cref="Project" /> from the <paramref name="assemblyName" />.
	/// </summary>
	/// <param name="assemblyName"></param>
	public ProjectReference(AssemblyName assemblyName)
	{
		_assemblyName = assemblyName;
	}
}
