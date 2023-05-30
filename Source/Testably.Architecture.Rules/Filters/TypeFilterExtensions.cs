using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     Extension methods for <see cref="ITypeFilter" />.
/// </summary>
public static partial class TypeFilterExtensions
{
	/// <summary>
	///     Filters the applicable <see cref="Type" /> on which the expectations should be applied.
	/// </summary>
	/// <param name="this">The <see cref="ITypeFilter" />.</param>
	/// <param name="filter">The filter to apply on the <see cref="Type" />.</param>
	public static ITypeFilterResult Which(this ITypeFilter @this,
		Expression<Func<Type, bool>> filter)
	{
		return @this.Which(Filter.FromPredicate(filter));
	}

	/// <summary>
	///     Filters the applicable <see cref="Type" /> on which the expectations should be applied.
	/// </summary>
	/// <param name="this">The <see cref="ITypeFilter" />.</param>
	/// <param name="filter">The filter to apply on the <see cref="Type" />.</param>
	/// <param name="name">The name of the filter.</param>
	public static ITypeFilterResult Which(this ITypeFilter @this,
		Func<Type, bool> filter,
		string name)
	{
		return @this.Which(Filter.FromPredicate(filter, name));
	}

	/// <summary>
	///     Filters the applicable <see cref="Type" /> for types which <see cref="Type.GetConstructors()" /> satisfy the
	///     <paramref name="constructorFilter" />.
	/// </summary>
	/// <param name="this">The <see cref="ITypeFilter" />.</param>
	/// <param name="constructorFilter">The filter to apply on the constructors of the <see cref="Type" />.</param>
	public static ITypeFilterResult Which(
		this ITypeFilter @this,
		IConstructorFilterResult constructorFilter)
	{
		return @this.Which(Filter.Delegate<Type, ConstructorInfo>(
			type => type.GetDeclaredConstructors(),
			constructorFilter,
			$"has constructors whose {constructorFilter}"));
	}

	/// <summary>
	///     Filters the applicable <see cref="Type" /> for types which <see cref="Type.GetEvents()" /> satisfy the
	///     <paramref name="eventFilter" />.
	/// </summary>
	/// <param name="this">The <see cref="ITypeFilter" />.</param>
	/// <param name="eventFilter">The filter to apply on the events of the <see cref="Type" />.</param>
	public static ITypeFilterResult Which(
		this ITypeFilter @this,
		IEventFilterResult eventFilter)
	{
		return @this.Which(Filter.Delegate<Type, EventInfo>(
			type => type.GetEvents(),
			eventFilter,
			$"has events whose {eventFilter}"));
	}

	/// <summary>
	///     Filters the applicable <see cref="Type" /> for types which <see cref="Type.GetFields()" /> satisfy the
	///     <paramref name="fieldFilter" />.
	/// </summary>
	/// <param name="this">The <see cref="ITypeFilter" />.</param>
	/// <param name="fieldFilter">The filter to apply on the fields of the <see cref="Type" />.</param>
	public static ITypeFilterResult Which(
		this ITypeFilter @this,
		IFieldFilterResult fieldFilter)
	{
		return @this.Which(Filter.Delegate<Type, FieldInfo>(
			type => type.GetDeclaredFields(),
			fieldFilter,
			$"has fields whose {fieldFilter}"));
	}

	/// <summary>
	///     Filters the applicable <see cref="Type" /> for types which <see cref="Type.GetMethods()" /> satisfy the
	///     <paramref name="methodFilter" />.
	/// </summary>
	/// <param name="this">The <see cref="ITypeFilter" />.</param>
	/// <param name="methodFilter">The filter to apply on the methods of the <see cref="Type" />.</param>
	public static ITypeFilterResult Which(
		this ITypeFilter @this,
		IMethodFilterResult methodFilter)
	{
		return @this.Which(Filter.Delegate<Type, MethodInfo>(
			type => type.GetDeclaredMethods(),
			methodFilter,
			$"has method whose {methodFilter}"));
	}

	/// <summary>
	///     Filters the applicable <see cref="Type" /> for types which <see cref="Type.GetProperties()" /> satisfy the
	///     <paramref name="propertyFilter" />.
	/// </summary>
	/// <param name="this">The <see cref="ITypeFilter" />.</param>
	/// <param name="propertyFilter">The filter to apply on the properties of the <see cref="Type" />.</param>
	public static ITypeFilterResult Which(
		this ITypeFilter @this,
		IPropertyFilterResult propertyFilter)
	{
		return @this.Which(Filter.Delegate<Type, PropertyInfo>(
			type => type.GetProperties(),
			propertyFilter,
			$"has property whose {propertyFilter}"));
	}
}
