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
		=> new ConstructorRule();

	/// <summary>
	///     Applies conditions on the <see cref="Type.GetEvents()" />.
	/// </summary>
	public static IEventFilter Event
		=> new EventRule();

	/// <summary>
	///     Applies conditions on the <see cref="Type.GetFields()" />.
	/// </summary>
	public static IFieldFilter Field
		=> new FieldRule();

	/// <summary>
	///     Applies conditions on the <see cref="Type.GetMethods()" />.
	/// </summary>
	public static IMethodFilter Method
		=> new MethodRule();

	/// <summary>
	///     Applies conditions on the <see cref="Type.GetProperties()" />.
	/// </summary>
	public static IPropertyFilter Property
		=> new PropertyRule();
}
