using Testably.Architecture.Rules.Internal;

namespace Testably.Architecture.Rules;

/// <summary>
///     Allows providing filters on elements of a <see cref="System.Type" />.
/// </summary>
public static class Have
{
	/// <summary>
	///     Applies conditions on the <see cref="System.Type.GetConstructors()" />.
	/// </summary>
	public static IConstructorFilter Constructor
		=> new ConstructorFilter();

	/// <summary>
	///     Applies conditions on the <see cref="System.Type.GetEvents()" />.
	/// </summary>
	public static IEventFilter Event
		=> new EventFilter();

	/// <summary>
	///     Applies conditions on the <see cref="System.Type.GetFields()" />.
	/// </summary>
	public static IFieldFilter Field
		=> new FieldFilter();

	/// <summary>
	///     Applies conditions on the <see cref="System.Type.GetMethods()" />.
	/// </summary>
	public static IMethodFilter Method
		=> new MethodFilter();

	/// <summary>
	///     Applies conditions on the <see cref="System.Type.GetProperties()" />.
	/// </summary>
	public static IPropertyFilter Property
		=> new PropertyFilter();
}
