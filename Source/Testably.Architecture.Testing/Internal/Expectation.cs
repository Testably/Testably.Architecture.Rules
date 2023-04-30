using System;
using System.Reflection;

namespace Testably.Architecture.Testing.Internal;

internal class Expectation : IExpectation
{
	#region IExpectation Members

	/// <inheritdoc cref="IExpectation.Assembly" />
	public IFilterableAssemblyExpectation Assembly(params Assembly[] assemblies) =>
		new AssemblyExpectation(assemblies);

	/// <inheritdoc cref="IExpectation.Type" />
	public IFilterableTypeExpectation Type(params Type[] types) =>
		new TypeExpectation(types);

	#endregion
}
