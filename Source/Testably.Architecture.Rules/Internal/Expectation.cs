namespace Testably.Architecture.Rules.Internal;

internal class Expectation : IExpectation
{
	#region IExpectation Members

	/// <inheritdoc cref="IExpectation.Assemblies" />
	public IAssemblyExpectation Assemblies
		=> new AssemblyRule();

	/// <inheritdoc cref="IExpectation.Constructors" />
	public IConstructorExpectation Constructors
		=> new ConstructorRule();

	/// <inheritdoc cref="IExpectation.Events" />
	public IEventExpectation Events
		=> new EventRule();

	/// <inheritdoc cref="IExpectation.Fields" />
	public IFieldExpectation Fields
		=> new FieldRule();

	/// <inheritdoc cref="IExpectation.Properties" />
	public IPropertyExpectation Properties
		=> new PropertyRule();

	/// <inheritdoc cref="IExpectation.Methods" />
	public IMethodExpectation Methods
		=> new MethodRule();

	/// <inheritdoc cref="IExpectation.Types" />
	public ITypeExpectation Types
		=> new TypeRule();

	#endregion
}
