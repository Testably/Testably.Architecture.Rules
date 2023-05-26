using System;
using System.Linq;
using System.Reflection;

namespace Testably.Architecture.Rules.Tests.TestHelpers;

internal static class FilterExtensions
{
	public static IConstructorFilterResult WhichAre(this IConstructorFilter constructorFilter,
		params ConstructorInfo[] methods)
		=> constructorFilter.Which(m => methods.Contains(m));

	public static IEventFilterResult WhichAre(this IEventFilter eventFilter,
		params EventInfo[] methods)
		=> eventFilter.Which(m => methods.Contains(m));

	public static IFieldFilterResult WhichAre(this IFieldFilter fieldFilter,
		params FieldInfo[] methods)
		=> fieldFilter.Which(m => methods.Contains(m));

	public static IMethodFilterResult WhichAre(this IMethodFilter methodFilter,
		params MethodInfo[] methods)
		=> methodFilter.Which(m => methods.Contains(m));

	public static IPropertyFilterResult WhichAre(this IPropertyFilter propertyFilter,
		params PropertyInfo[] methods)
		=> propertyFilter.Which(m => methods.Contains(m));

	public static ITypeFilterResult WhichAre(this ITypeFilter typeFilter, params Type[] types)
		=> typeFilter.Which(t => types.Contains(t));
}
