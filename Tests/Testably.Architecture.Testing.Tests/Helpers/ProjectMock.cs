using Testably.Architecture.Testing.Models;

namespace Testably.Architecture.Testing.Tests.Helpers;

internal class ProjectMock : Project
{
	/// <inheritdoc cref="Project.Name" />
	public override string Name { get; }

	/// <inheritdoc cref="Project.ProjectReferences" />
	public override ProjectReference[] ProjectReferences { get; }

	public ProjectMock(string name, params ProjectReference[] projectReferences)
		: base(null!)
	{
		Name = name;
		ProjectReferences = projectReferences;
	}
}
