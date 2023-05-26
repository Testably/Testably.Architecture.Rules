using System;

namespace Testably.Architecture.Rules;

public static partial class RequirementOnTypeExtensions
{
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
			type => type.InheritsFrom(baseType, forceDirect),
			type => new TypeTestError(type,
				$"The type '{type.Name}' should{(forceDirect ? " directly" : "")} inherit from '{baseType.Name}'.")));

	/// <summary>
	///     Expect the types to not inherit from <typeparamref name="TBase" />.
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
	///     Expect the types to not inherit from <paramref name="baseType" />.
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
			type => !type.InheritsFrom(baseType, forceDirect),
			type => new TypeTestError(type,
				$"The type '{type.Name}' should not{(forceDirect ? " directly" : "")} inherit from '{baseType.Name}'.")));
}
