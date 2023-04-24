using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Testably.Architecture.Testing.Models;

namespace Testably.Architecture.Testing.Internal;

internal class AssembliesProjectExpectation : ProjectExpectation
{
	public AssembliesProjectExpectation(IEnumerable<Assembly> projects)
		: base(projects.Select(p => new Project(p)))
	{
	}
}
