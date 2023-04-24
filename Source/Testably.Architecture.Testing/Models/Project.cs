using System.Linq;
using System.Reflection;

namespace Testably.Architecture.Testing.Models;

/// <summary>
///     Project information gathered from an <see cref="Assembly" />.
/// </summary>
public class Project
{
	/// <summary>
	///     The name of the project.
	/// </summary>
	public virtual string Name => _assembly.GetName().Name ?? _assembly.ToString();

	/// <summary>
	///     The referenced projects that this project depends upon.
	/// </summary>
	public virtual ProjectReference[] ProjectReferences
		=> _projectReferences ??= GetProjectReferences();

	private readonly Assembly _assembly;
	private ProjectReference[]? _projectReferences;

	/// <summary>
	///     Initializes a new instance of <see cref="Project" /> from the <paramref name="assembly" />.
	/// </summary>
	/// <param name="assembly"></param>
	public Project(Assembly assembly)
	{
		_assembly = assembly;
	}

	/// <inheritdoc cref="object.ToString()" />
	public override string ToString()
		=> Name;

	private ProjectReference[] GetProjectReferences()
	{
		return _assembly.GetReferencedAssemblies()
			.Select(x => new ProjectReference(x))
			.ToArray();
	}
}
