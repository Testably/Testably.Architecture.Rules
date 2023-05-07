using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     Extension methods for <see cref="IRequirement{Type}" />.
/// </summary>
public static class RequirementOnTypeExtensions
{
	/// <summary>
	///     Expect the types to be abstract.
	/// </summary>
	/// <param name="this">The <see cref="IRequirement{Type}" />.</param>
	public static IRequirementResult<Type> ShouldBeAbstract(
		this IRequirement<Type> @this)
		=> @this.ShouldSatisfy(Requirement.ForType(
			type => type.IsAbstract,
			type => new TypeTestError(type,
				$"Type '{type.Name}' should be abstract.")));

	/// <summary>
	///     Expect the types to be a class.
	/// </summary>
	/// <param name="this">The <see cref="IRequirement{Type}" />.</param>
	public static IRequirementResult<Type> ShouldBeAClass(
		this IRequirement<Type> @this)
		=> @this.ShouldSatisfy(Requirement.ForType(
			type => type.IsClass,
			type => new TypeTestError(type,
				$"Type '{type.Name}' should be a class.")));

	/// <summary>
	///     Expect the types to be an interface.
	/// </summary>
	/// <param name="this">The <see cref="IRequirement{Type}" />.</param>
	public static IRequirementResult<Type> ShouldBeAnInterface(
		this IRequirement<Type> @this)
		=> @this.ShouldSatisfy(Requirement.ForType(
			type => type.IsInterface,
			type => new TypeTestError(type,
				$"Type '{type.Name}' should be an interface.")));

	/// <summary>
	///     Expect the types to be generic.
	/// </summary>
	/// <param name="this">The <see cref="IRequirement{Type}" />.</param>
	public static IRequirementResult<Type> ShouldBeGeneric(
		this IRequirement<Type> @this)
		=> @this.ShouldSatisfy(Requirement.ForType(
			type => type.IsGenericType,
			type => new TypeTestError(type,
				$"Type '{type.Name}' should be generic.")));

	/// <summary>
	///     Expect the types to be nested.
	/// </summary>
	/// <param name="this">The <see cref="IRequirement{Type}" />.</param>
	public static IRequirementResult<Type> ShouldBeNested(
		this IRequirement<Type> @this)
		=> @this.ShouldSatisfy(Requirement.ForType(
			type => type.IsNested,
			type => new TypeTestError(type,
				$"Type '{type.Name}' should be nested.")));

	/// <summary>
	///     Expect the types to be public.
	/// </summary>
	/// <param name="this">The <see cref="IRequirement{Type}" />.</param>
	public static IRequirementResult<Type> ShouldBePublic(
		this IRequirement<Type> @this)
		=> @this.ShouldSatisfy(Requirement.ForType(
			type => type.IsPublic,
			type => new TypeTestError(type,
				$"Type '{type.Name}' should be public.")));

	/// <summary>
	///     Expect the types to be sealed.
	/// </summary>
	/// <param name="this">The <see cref="IRequirement{Type}" />.</param>
	public static IRequirementResult<Type> ShouldBeSealed(
		this IRequirement<Type> @this)
		=> @this.ShouldSatisfy(Requirement.ForType(
			type => type.IsSealed,
			type => new TypeTestError(type,
				$"Type '{type.Name}' should be sealed.")));

	/// <summary>
	///     Expect the types to be static.
	/// </summary>
	/// <param name="this">The <see cref="IRequirement{Type}" />.</param>
	public static IRequirementResult<Type> ShouldBeStatic(
		this IRequirement<Type> @this)
		=> @this.ShouldSatisfy(Requirement.ForType(
			type => type.IsStatic(),
			type => new TypeTestError(type,
				$"Type '{type.Name}' should be static.")));

	/// <summary>
	///     Expect the types to have an attribute of type <typeparamref name="TAttribute" />.
	/// </summary>
	/// <param name="this">The <see cref="IRequirement{Type}" />.</param>
	/// <param name="predicate">
	///     (optional) A predicate to check the attribute values.
	///     <para />
	///     If not set (<see langword="null" />), will only check if the attribute is present.
	/// </param>
	/// <param name="inherit">
	///     <see langword="true" /> to search the inheritance chain to find the attributes; otherwise,
	///     <see langword="false" />.<br />
	///     Defaults to <see langword="true" />
	/// </param>
	public static IRequirementResult<Type> ShouldHaveAttribute<TAttribute>(
		this IRequirement<Type> @this,
		Func<TAttribute, bool>? predicate = null,
		bool inherit = true)
		where TAttribute : Attribute
		=> @this.ShouldSatisfy(Requirement.ForType(
			type => type.HasAttribute(predicate, inherit),
			type => new TypeTestError(type,
				$"Type '{type.Name}' should have correct attribute '{typeof(TAttribute).Name}'.")));

	/// <summary>
	///     Expect the types to inherit from <typeparamref name="TBase" />.
	/// </summary>
	/// <param name="this">The <see cref="IRequirement{Type}" />.</param>
	/// <param name="forceDirect">
	///     If set to <see langword="false" /> (default value), the <typeparamref name="TBase" />
	///     can be anywhere in the inheritance tree, otherwise if set to <see langword="true" /> requires the
	///     <typeparamref name="TBase" /> to be the direct parent.
	/// </param>
	public static IRequirementResult<Type> ShouldInheritFrom<TBase>(
		this IRequirement<Type> @this,
		bool forceDirect = false)
		=> @this.ShouldInheritFrom(typeof(TBase), forceDirect);

	/// <summary>
	///     Expect the types to inherit from <paramref name="baseType" />.
	/// </summary>
	/// <param name="this">The <see cref="IRequirement{Type}" />.</param>
	/// <param name="baseType">The base type which should be inherited from.</param>
	/// <param name="forceDirect">
	///     If set to <see langword="false" /> (default value), the <paramref name="baseType" />
	///     can be anywhere in the inheritance tree, otherwise if set to <see langword="true" /> requires the
	///     <paramref name="baseType" /> to be the direct parent.
	/// </param>
	public static IRequirementResult<Type> ShouldInheritFrom(
		this IRequirement<Type> @this,
		Type baseType,
		bool forceDirect = false)
		=> @this.ShouldSatisfy(Requirement.ForType(
			type => type.Inherits(baseType, forceDirect),
			type => new TypeTestError(type,
				$"Type '{type.Name}' should{(forceDirect ? " directly" : "")} inherit from '{baseType.Name}'.")));

	/// <summary>
	///     Expect the <see cref="MemberInfo.Name" /> of the types to match the given <paramref name="pattern" />.
	/// </summary>
	/// <param name="this">The <see cref="IRequirement{Type}" />.</param>
	/// <param name="pattern">
	///     The wildcard condition.
	///     <para />
	///     Supports * to match zero or more characters and ? to match exactly one character.
	/// </param>
	/// <param name="ignoreCase">Flag indicating if the comparison should be case sensitive or not.</param>
	public static IRequirementResult<Type> ShouldMatchName(
		this IRequirement<Type> @this,
		Match pattern,
		bool ignoreCase = false)
		=> @this.ShouldSatisfy(Requirement.ForType(
			type => pattern.Matches(type.Name, ignoreCase),
			type => new TypeTestError(type,
				$"Type '{type.Name}' should match pattern '{pattern}'.")));

	/// <summary>
	///     Expect the types to not be abstract.
	/// </summary>
	/// <param name="this">The <see cref="IRequirement{Type}" />.</param>
	public static IRequirementResult<Type> ShouldNotBeAbstract(
		this IRequirement<Type> @this)
		=> @this.ShouldSatisfy(Requirement.ForType(
			type => !type.IsAbstract,
			type => new TypeTestError(type,
				$"Type '{type.Name}' should not be abstract.")));

	/// <summary>
	///     Expect the types to not be a class.
	/// </summary>
	/// <param name="this">The <see cref="IRequirement{Type}" />.</param>
	public static IRequirementResult<Type> ShouldNotBeAClass(
		this IRequirement<Type> @this)
		=> @this.ShouldSatisfy(Requirement.ForType(
			type => !type.IsClass,
			type => new TypeTestError(type,
				$"Type '{type.Name}' should not be a class.")));

	/// <summary>
	///     Expect the types to not be an interface.
	/// </summary>
	/// <param name="this">The <see cref="IRequirement{Type}" />.</param>
	public static IRequirementResult<Type> ShouldNotBeAnInterface(
		this IRequirement<Type> @this)
		=> @this.ShouldSatisfy(Requirement.ForType(
			type => !type.IsInterface,
			type => new TypeTestError(type,
				$"Type '{type.Name}' should not be an interface.")));

	/// <summary>
	///     Expect the types to not be generic.
	/// </summary>
	/// <param name="this">The <see cref="IRequirement{Type}" />.</param>
	public static IRequirementResult<Type> ShouldNotBeGeneric(
		this IRequirement<Type> @this)
		=> @this.ShouldSatisfy(Requirement.ForType(
			type => !type.IsGenericType,
			type => new TypeTestError(type,
				$"Type '{type.Name}' should not be generic.")));

	/// <summary>
	///     Expect the types to not be nested.
	/// </summary>
	/// <param name="this">The <see cref="IRequirement{Type}" />.</param>
	public static IRequirementResult<Type> ShouldNotBeNested(
		this IRequirement<Type> @this)
		=> @this.ShouldSatisfy(Requirement.ForType(
			type => !type.IsNested,
			type => new TypeTestError(type,
				$"Type '{type.Name}' should not be nested.")));

	/// <summary>
	///     Expect the types to not be public.
	/// </summary>
	/// <param name="this">The <see cref="IRequirement{Type}" />.</param>
	public static IRequirementResult<Type> ShouldNotBePublic(
		this IRequirement<Type> @this)
		=> @this.ShouldSatisfy(Requirement.ForType(
			type => !type.IsPublic,
			type => new TypeTestError(type,
				$"Type '{type.Name}' should not be public.")));

	/// <summary>
	///     Expect the types to not be sealed.
	/// </summary>
	/// <param name="this">The <see cref="IRequirement{Type}" />.</param>
	public static IRequirementResult<Type> ShouldNotBeSealed(
		this IRequirement<Type> @this)
		=> @this.ShouldSatisfy(Requirement.ForType(
			type => !type.IsSealed,
			type => new TypeTestError(type,
				$"Type '{type.Name}' should not be sealed.")));

	/// <summary>
	///     Expect the types to not be static.
	/// </summary>
	/// <param name="this">The <see cref="IRequirement{Type}" />.</param>
	public static IRequirementResult<Type> ShouldNotBeStatic(
		this IRequirement<Type> @this)
		=> @this.ShouldSatisfy(Requirement.ForType(
			type => !type.IsStatic(),
			type => new TypeTestError(type,
				$"Type '{type.Name}' should not be static.")));

	/// <summary>
	///     Expect the types to not have an attribute of type <typeparamref name="TAttribute" />.
	/// </summary>
	/// <param name="this">The <see cref="IRequirement{Type}" />.</param>
	/// <param name="predicate">
	///     (optional) A predicate to check the attribute values.
	///     <para />
	///     If not set (<see langword="null" />), will only check if the attribute is present.
	/// </param>
	/// <param name="inherit">
	///     <see langword="true" /> to search the inheritance chain to find the attributes; otherwise,
	///     <see langword="false" />.<br />
	///     Defaults to <see langword="true" />
	/// </param>
	public static IRequirementResult<Type> ShouldNotHaveAttribute<TAttribute>(
		this IRequirement<Type> @this,
		Func<TAttribute, bool>? predicate = null,
		bool inherit = true)
		where TAttribute : Attribute
		=> @this.ShouldSatisfy(Requirement.ForType(
			type => !type.HasAttribute(predicate, inherit),
			type => new TypeTestError(type,
				$"Type '{type.Name}' should not have correct attribute '{typeof(TAttribute).Name}'.")));

	/// <summary>
	///     Expect the types to inherit from <typeparamref name="TBase" />.
	/// </summary>
	/// <param name="this">The <see cref="IRequirement{Type}" />.</param>
	/// <param name="forceDirect">
	///     If set to <see langword="false" /> (default value), the <typeparamref name="TBase" />
	///     can be anywhere in the inheritance tree, otherwise if set to <see langword="true" /> requires the
	///     <typeparamref name="TBase" /> to be the direct parent.
	/// </param>
	public static IRequirementResult<Type> ShouldNotInheritFrom<TBase>(
		this IRequirement<Type> @this,
		bool forceDirect = false)
		=> @this.ShouldNotInheritFrom(typeof(TBase), forceDirect);

	/// <summary>
	///     Expect the types to inherit from <paramref name="baseType" />.
	/// </summary>
	/// <param name="this">The <see cref="IRequirement{Type}" />.</param>
	/// <param name="baseType">The base type which should be inherited from.</param>
	/// <param name="forceDirect">
	///     If set to <see langword="false" /> (default value), the <paramref name="baseType" />
	///     can be anywhere in the inheritance tree, otherwise if set to <see langword="true" /> requires the
	///     <paramref name="baseType" /> to be the direct parent.
	/// </param>
	public static IRequirementResult<Type> ShouldNotInheritFrom(
		this IRequirement<Type> @this,
		Type baseType,
		bool forceDirect = false)
		=> @this.ShouldSatisfy(Requirement.ForType(
			type => !type.Inherits(baseType, forceDirect),
			type => new TypeTestError(type,
				$"Type '{type.Name}' should not{(forceDirect ? " directly" : "")} inherit from '{baseType.Name}'.")));

	/// <summary>
	///     Expect the <see cref="MemberInfo.Name" /> of the types to not match the given <paramref name="pattern" />.
	/// </summary>
	/// <param name="this">The <see cref="IRequirement{Type}" />.</param>
	/// <param name="pattern">
	///     The wildcard condition.
	///     <para />
	///     Supports * to match zero or more characters and ? to match exactly one character.
	/// </param>
	/// <param name="ignoreCase">Flag indicating if the comparison should be case sensitive or not.</param>
	public static IRequirementResult<Type> ShouldNotMatchName(
		this IRequirement<Type> @this,
		Match pattern,
		bool ignoreCase = false)
		=> @this.ShouldSatisfy(Requirement.ForType(
			type => !pattern.Matches(type.Name, ignoreCase),
			type => new TypeTestError(type,
				$"Type '{type.Name}' not match pattern '{pattern}'.")));

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
				$"Type '{type.FullName}' should satisfy the required condition {condition}.")));
	}
}
