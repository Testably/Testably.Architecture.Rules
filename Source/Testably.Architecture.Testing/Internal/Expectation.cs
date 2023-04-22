using System.Linq;
using System.Reflection;

namespace Testably.Architecture.Testing.Internal;

internal class Expectation : IExpectation
{
	/// <inheritdoc cref="IExpectation.FromAssembly(Assembly[])" />
	public IProjectExpectation FromAssembly(params Assembly[] assemblies)
	{
		return new CompositeAssemblyContainer(assemblies
			.Select(x => new AssemblyContainer(x)));
	}
}
