using System;
using System.Linq;
using System.Reflection;

namespace Testably.Architecture.Rules.Tests.TestHelpers;

internal static class FilterExtensions
{
	public static IConstructorFilterResult WhichAre(this IConstructorFilter constructorFilter,
		params ConstructorInfo[] constructors)
		=> constructorFilter.Which(c => constructors.Contains(c));

	public static IEventFilterResult WhichAre(this IEventFilter eventFilter,
		params EventInfo[] events)
		=> eventFilter.Which(e => events.Contains(e));

	public static IFieldFilterResult WhichAre(this IFieldFilter fieldFilter,
		params FieldInfo[] fields)
		=> fieldFilter.Which(f => fields.Contains(f));

	public static IMethodFilterResult WhichAre(this IMethodFilter methodFilter,
		params MethodInfo[] methods)
		=> methodFilter.Which(m => methods.Contains(m));

	public static IPropertyFilterResult WhichAre(this IPropertyFilter propertyFilter,
		params PropertyInfo[] properties)
		=> propertyFilter.Which(p => properties.Contains(p));

	public static ITypeFilterResult WhichAre(this ITypeFilter typeFilter, params Type[] types)
		=> typeFilter.Which(t => types.Contains(t));
}
