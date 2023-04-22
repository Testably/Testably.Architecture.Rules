using System;
using System.Linq;

namespace Testably.Architecture.Testing.Internal;

internal class Expectation : IExpectation
{
	#region IExpectation Members

	public IProjectExpectation AllLoadedProjects()
	{
		return new CompositeAssemblyContainer(AppDomain.CurrentDomain.GetAssemblies()
			.Select(x => new AssemblyContainer(x)));
	}

	public IProjectExpectation ProjectContaining<TAssembly>()
	{
		return new AssemblyContainer(typeof(TAssembly).Assembly);
	}

	#endregion
}
