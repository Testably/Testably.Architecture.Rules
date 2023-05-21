using System;

namespace Testably.Architecture.Rules;

public static partial class RequirementOnTypeExtensions
{
	/// <summary>
	///     Expect the types to implement the <typeparamref name="TInterface" />.
	/// </summary>
	/// <typeparamref name="TInterface">The type of the interface which should be implemented.</typeparamref>
	/// <param name="this">The <see cref="IRequirement{Type}" />.</param>
	/// <param name="forceDirect">
	///     If set to <see langword="false" /> (default value), the <typeparamref name="TInterface" />
	///     can be anywhere in the inheritance tree, otherwise if set to <see langword="true" /> requires the
	///     <typeparamref name="TInterface" /> to be directly implemented.
	/// </param>
	public static IRequirementResult<Type> ShouldImplement<TInterface>(
		this IRequirement<Type> @this,
		bool forceDirect = false)
		=> @this.ShouldImplement(typeof(TInterface), forceDirect);

	/// <summary>
	///     Expect the types to implement the <paramref name="interfaceType" />.
	/// </summary>
	/// <param name="this">The <see cref="IRequirement{Type}" />.</param>
	/// <param name="interfaceType">The interface which should be implemented.</param>
	/// <param name="forceDirect">
	///     If set to <see langword="false" /> (default value), the <paramref name="interfaceType" />
	///     can be anywhere in the inheritance tree, otherwise if set to <see langword="true" /> requires the
	///     <paramref name="interfaceType" /> to be directly implemented.
	/// </param>
	public static IRequirementResult<Type> ShouldImplement(
		this IRequirement<Type> @this,
		Type interfaceType,
		bool forceDirect = false)
		=> @this.ShouldSatisfy(Requirement.ForType(
			type => type.Implements(interfaceType, forceDirect),
			type => new TypeTestError(type,
				$"Type '{type.Name}' should{(forceDirect ? " directly" : "")} implement '{interfaceType.Name}'.")));

	/// <summary>
	///     Expect the types to not implement the <typeparamref name="TInterface" />.
	/// </summary>
	/// <typeparamref name="TInterface">The type of the interface which should be implemented.</typeparamref>
	/// <param name="this">The <see cref="IRequirement{Type}" />.</param>
	/// <param name="forceDirect">
	///     If set to <see langword="false" /> (default value), the <typeparamref name="TInterface" />
	///     can be anywhere in the inheritance tree, otherwise if set to <see langword="true" /> requires the
	///     <typeparamref name="TInterface" /> to be directly implemented.
	/// </param>
	public static IRequirementResult<Type> ShouldNotImplement<TInterface>(
		this IRequirement<Type> @this,
		bool forceDirect = false)
		=> @this.ShouldNotImplement(typeof(TInterface), forceDirect);

	/// <summary>
	///     Expect the types to not implement the <paramref name="interfaceType" />.
	/// </summary>
	/// <param name="this">The <see cref="IRequirement{Type}" />.</param>
	/// <param name="interfaceType">The interface which should be implemented.</param>
	/// <param name="forceDirect">
	///     If set to <see langword="false" /> (default value), the <paramref name="interfaceType" />
	///     can be anywhere in the inheritance tree, otherwise if set to <see langword="true" /> requires the
	///     <paramref name="interfaceType" /> to be directly implemented.
	/// </param>
	public static IRequirementResult<Type> ShouldNotImplement(
		this IRequirement<Type> @this,
		Type interfaceType,
		bool forceDirect = false)
		=> @this.ShouldSatisfy(Requirement.ForType(
			type => !type.Implements(interfaceType, forceDirect),
			type => new TypeTestError(type,
				$"Type '{type.Name}' should not{(forceDirect ? " directly" : "")} implement '{interfaceType.Name}'.")));
}
