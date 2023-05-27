﻿using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     Extension methods for <see cref="IRequirement{Type}" />.
/// </summary>
public static partial class RequirementOnTypeExtensions
{
	/// <summary>
	///     The <see cref="Type" /> should satisfy the given <paramref name="constructorFilter" />.
	/// </summary>
	public static IRequirementResult<Type> Should(
		this IRequirement<Type> @this,
		IConstructorFilterResult constructorFilter)
	{
		return @this.ShouldSatisfy(
			Requirement.Delegate<Type, ConstructorInfo>(
				type => type.GetConstructors(),
				type => Requirement.ForConstructor(constructorFilter.Applies,
					constructor => new ConstructorTestError(constructor,
						$"The type '{type.FullName}' should have a constructor {constructorFilter}"))));
	}

	/// <summary>
	///     The <see cref="Type" /> should satisfy the given <paramref name="eventFilter" />.
	/// </summary>
	public static IRequirementResult<Type> Should(
		this IRequirement<Type> @this,
		IEventFilterResult eventFilter)
	{
		return @this.ShouldSatisfy(
			Requirement.Delegate<Type, EventInfo>(
				type => type.GetEvents(),
				type => Requirement.ForEvent(eventFilter.Applies,
					@event => new EventTestError(@event,
						$"The type '{type.FullName}' should have an event whose {eventFilter}"))));
	}

	/// <summary>
	///     The <see cref="Type" /> should satisfy the given <paramref name="fieldFilter" />.
	/// </summary>
	public static IRequirementResult<Type> Should(
		this IRequirement<Type> @this,
		IFieldFilterResult fieldFilter)
	{
		return @this.ShouldSatisfy(
			Requirement.Delegate<Type, FieldInfo>(
				type => type.GetFields(),
				type => Requirement.ForField(fieldFilter.Applies,
					field => new FieldTestError(field,
						$"The type '{type.FullName}' should have a field {fieldFilter}"))));
	}

	/// <summary>
	///     The <see cref="Type" /> should satisfy the given <paramref name="methodFilter" />.
	/// </summary>
	public static IRequirementResult<Type> Should(
		this IRequirement<Type> @this,
		IMethodFilterResult methodFilter)
	{
		return @this.ShouldSatisfy(
			Requirement.Delegate<Type, MethodInfo>(
				type => type.GetDeclaredMethods(),
				type => Requirement.ForMethod(methodFilter.Applies,
					method => new MethodTestError(method,
						$"The type '{type.FullName}' should have a method {methodFilter}"))));
	}

	/// <summary>
	///     The <see cref="Type" /> should satisfy the given <paramref name="propertyFilter" />.
	/// </summary>
	public static IRequirementResult<Type> Should(
		this IRequirement<Type> @this,
		IPropertyFilterResult propertyFilter)
	{
		return @this.ShouldSatisfy(
			Requirement.Delegate<Type, PropertyInfo>(
				type => type.GetProperties(),
				type => Requirement.ForProperty(propertyFilter.Applies,
					property => new PropertyTestError(property,
						$"The type '{type.FullName}' should have a property {propertyFilter}"))));
	}

	/// <summary>
	///     The <see cref="Type" /> should satisfy the given <paramref name="condition" />.
	/// </summary>
	public static IRequirementResult<Type> ShouldSatisfy(
		this IRequirement<Type> @this,
		Expression<Func<Type, bool>> condition)
	{
		Func<Type, bool> compiledCondition = condition.Compile();
		return @this.ShouldSatisfy(Requirement.ForType(
			compiledCondition,
			type => new TypeTestError(type,
				$"The type '{type.FullName}' should satisfy the required condition {condition}.")));
	}
}
