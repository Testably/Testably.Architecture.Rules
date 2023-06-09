﻿using System;

namespace Testably.Architecture.Rules;

public static partial class RequirementOnTypeExtensions
{
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
				$"The type '{type.Name}' should have correct attribute '{typeof(TAttribute).Name}'.")));

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
				$"The type '{type.Name}' should not have correct attribute '{typeof(TAttribute).Name}'.")));
}
