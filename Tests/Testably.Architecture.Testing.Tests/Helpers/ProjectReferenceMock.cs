using Testably.Architecture.Testing.Models;

namespace Testably.Architecture.Testing.Tests.Helpers;

internal class ProjectReferenceMock : ProjectReference
{
	/// <inheritdoc cref="ProjectReference.Name" />
	public override string Name { get; }

	public ProjectReferenceMock(string name)
		: base(null!)
	{
		Name = name;
	}
}
