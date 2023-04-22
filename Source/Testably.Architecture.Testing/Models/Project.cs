using System.Reflection;

namespace Testably.Architecture.Testing.Models;

/// <summary>
///     Project information gathered from an <see cref="Assembly" />.
/// </summary>
public class Project
{
	private readonly Assembly _assembly;

	/// <summary>
	///     Initializes a new instance of <see cref="Project" /> from the <paramref name="assembly" />.
	/// </summary>
	/// <param name="assembly"></param>
	public Project(Assembly assembly)
	{
		_assembly = assembly;
	}

	/// <summary>
	///     The name of the project.
	/// </summary>
	public string Name => _assembly.GetName().Name ?? _assembly.ToString();
}
