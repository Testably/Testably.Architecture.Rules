using System;
using System.Reflection;

namespace Testably.Architecture.Testing.Internal;

internal class Expectation : IExpectation
{
	#region IExpectation Members

	/// <inheritdoc cref="IExpectation.Assembly" />
	public IAssemblyExpectation Assembly(params Assembly[] assemblies) =>
		new AssemblyExpectationStart(assemblies);

	/// <inheritdoc cref="IExpectation.Type" />
	public ITypeExpectation Type(params Type[] types) =>
		new TypeExpectationStart(types);

	#endregion
}
