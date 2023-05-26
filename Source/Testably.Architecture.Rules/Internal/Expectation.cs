namespace Testably.Architecture.Rules.Internal;

internal class Expectation : IExpectation
{
	#region IExpectation Members

	/// <inheritdoc cref="IExpectation.Assemblies" />
	public IAssemblyExpectation Assemblies
		=> new AssemblyRule();

	/// <inheritdoc cref="IExpectation.Methods" />
	public IMethodExpectation Methods
		=> new MethodRule();

	/// <inheritdoc cref="IExpectation.Types" />
	public ITypeExpectation Types
		=> new TypeRule();

	#endregion
}
