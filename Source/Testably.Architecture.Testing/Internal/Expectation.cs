using System.Reflection;

namespace Testably.Architecture.Testing.Internal;

internal class Expectation : IExpectation
{
	#region IExpectation Members

	/// <inheritdoc cref="IExpectation.FromAssembly(Assembly[])" />
	public IProjectExpectation FromAssembly(params Assembly[] assemblies)
	{
		return new AssembliesProjectExpectation(assemblies);
	}

	#endregion
}
