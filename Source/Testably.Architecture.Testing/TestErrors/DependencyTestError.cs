using System.Linq;
using Testably.Architecture.Testing.Models;

namespace Testably.Architecture.Testing.TestErrors;

public class DependencyTestError : TestError
{
	public Project Project { get; }
	public ProjectReference[] ProjectReferences { get; }

	public DependencyTestError(Project project, ProjectReference[] projectReferences)
		: base(
			$"Project {project.Name} has incorrect references: {string.Join(", ", projectReferences.Select(x => x.Name))}")
	{
		Project = project;
		ProjectReferences = projectReferences;
	}
}
