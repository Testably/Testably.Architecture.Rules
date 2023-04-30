using System;
using System.Reflection;

namespace Testably.Architecture.Testing.Internal;

internal class Expectation : IExpectation
{
	#region IExpectation Members

	/// <inheritdoc cref="IExpectation.Assemblies" />
	public IFilterableAssemblyExpectation Assemblies(params Assembly[] assemblies) =>
		new AssemblyExpectation(assemblies);

	/// <inheritdoc cref="IExpectation.Types" />
	public IFilterableTypeExpectation Types(params Type[] types) =>
		new TypeExpectation(types);

	#endregion
}
