using System;
using System.Reflection;
using Testably.Architecture.Rules.Internal;

namespace Testably.Architecture.Rules;

/// <summary>
///     Allows providing filters on <see cref="MemberInfo" />s of a <see cref="Type" />.
/// </summary>
public static class Have
{
	/// <summary>
	///     Applies conditions on the <see cref="Type.GetConstructors()" />.
	/// </summary>
	public static IConstructorFilter Constructor
		=> new ConstructorFilter();

	/// <summary>
	///     Applies conditions on the <see cref="Type.GetEvents()" />.
	/// </summary>
	public static IEventFilter Event
		=> new EventFilter();

	/// <summary>
	///     Applies conditions on the <see cref="Type.GetFields()" />.
	/// </summary>
	public static IFieldFilter Field
		=> new FieldFilter();

	/// <summary>
	///     Applies conditions on the <see cref="Type.GetMethods()" />.
	/// </summary>
	public static IMethodFilter Method
		=> new MethodFilter();

	/// <summary>
	///     Applies conditions on the <see cref="Type.GetProperties()" />.
	/// </summary>
	public static IPropertyFilter Property
		=> new PropertyFilter();
}
