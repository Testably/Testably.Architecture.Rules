using System.Reflection;

namespace Testably.Architecture.Testing.Models;

/// <summary>
///     Project reference information gathered from an <see cref="AssemblyName" />.
/// </summary>
public class ProjectReference
{
	/// <summary>
	///     The name of the referenced project.
	/// </summary>
	public virtual string Name => _assemblyName.Name ?? _assemblyName.ToString();

	private readonly AssemblyName _assemblyName;

	/// <summary>
	///     Initializes a new instance of <see cref="ProjectReference" /> from the <paramref name="assemblyName" />.
	/// </summary>
	/// <param name="assemblyName"></param>
	public ProjectReference(AssemblyName assemblyName)
	{
		_assemblyName = assemblyName;
	}

	/// <inheritdoc cref="object.ToString()" />
	public override string ToString()
		=> Name;
}
