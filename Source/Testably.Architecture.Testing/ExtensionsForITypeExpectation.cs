﻿using System;
using System.Linq.Expressions;
using Testably.Architecture.Testing.TestErrors;

namespace Testably.Architecture.Testing;

/// <summary>
///     Extension methods for <see cref="ITypeExpectation" />.
/// </summary>
public static class ExtensionsForITypeExpectation
{
	/// <summary>
	///     Expect the types to be sealed.
	/// </summary>
	/// <param name="this">The <see cref="ITypeExpectation" />.</param>
	/// <param name="isSealed">
	///     The expected value.<br />
	///     Defaults to <see langword="true" />.
	/// </param>
	public static ITestResult<ITypeExpectation> ShouldBeSealed(
		this ITypeExpectation @this,
		bool isSealed = true)
		=> @this.ShouldSatisfy(type => type.IsSealed == isSealed,
			type => new TypeTestError(type, $"Type '{type.Name}' is not sealed."));

	/// <summary>
	///     Expect the types to have an attribute of type <typeparamref name="TAttribute" />.
	/// </summary>
	/// <param name="this">The <see cref="ITypeExpectation" />.</param>
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
	public static ITestResult<ITypeExpectation> ShouldHaveAttribute<TAttribute>(
		this ITypeExpectation @this,
		Func<TAttribute, bool>? predicate = null,
		bool inherit = true)
		where TAttribute : Attribute
		=> @this.ShouldSatisfy(type => type.HasAttribute(predicate, inherit),
			type => new TypeTestError(type,
				$"Type '{type.Name}' does not have correct attribute '{typeof(TAttribute).Name}'."));

	/// <summary>
	///     The <see cref="Type" /> should satisfy the given <paramref name="condition" />.
	/// </summary>
	public static ITestResult<ITypeExpectation> ShouldSatisfy(
		this ITypeExpectation @this,
		Expression<Func<Type, bool>> condition)
	{
		Func<Type, bool> compiledCondition = condition.Compile();
		return @this.ShouldSatisfy(compiledCondition,
			type => new TypeTestError(type,
				$"Type '{type.Name}' does not satisfy the required condition {condition}."));
	}
}