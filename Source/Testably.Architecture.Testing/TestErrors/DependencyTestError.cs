using System.Linq;
using Testably.Architecture.Testing.Models;

namespace Testably.Architecture.Testing.TestErrors;

/// <summary>
///     A <see cref="TestError" /> due to an incorrect dependency.
/// </summary>
public class DependencyTestError : TestError
{
	/// <summary>
	///     The <see cref="Models.Project" /> which has the incorrect dependency.
	/// </summary>
	public Project Project { get; }

	/// <summary>
	///     The <see cref="ProjectReference" />s that are incorrect in the <see cref="Project" />.
	/// </summary>
	public ProjectReference[] ProjectReferences { get; }

	/// <summary>
	///     Initializes a new instance of <see cref="DependencyTestError" />.
	/// </summary>
	/// <param name="project">The <see cref="Models.Project" /> which has the incorrect dependency.</param>
	/// <param name="projectReferences">
	///     The <see cref="ProjectReference" />s that are incorrect in the
	///     <paramref name="project" />.
	/// </param>
	public DependencyTestError(Project project, ProjectReference[] projectReferences)
		: base(
			$"Project {project.Name} has incorrect references: {string.Join(", ", projectReferences.Select(x => x.Name))}")
	{
		Project = project;
		ProjectReferences = projectReferences;
	}
}
