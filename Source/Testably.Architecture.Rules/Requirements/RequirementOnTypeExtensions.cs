using System;
using System.Linq.Expressions;

namespace Testably.Architecture.Rules;

/// <summary>
///     Extension methods for <see cref="IRequirement{Type}" />.
/// </summary>
public static partial class RequirementOnTypeExtensions
{
	/// <summary>
	///     The <see cref="Type" /> should satisfy the given <paramref name="methodFilter" />.
	/// </summary>
	public static IRequirementResult<Type> Should(
		this IRequirement<Type> @this,
		IMethodFilterResult methodFilter)
	{
		Filter<Type> typeFilter = methodFilter.ToTypeFilter();
		return @this.ShouldSatisfy(Requirement.ForType(
			typeFilter.Applies,
			type => new TypeTestError(type,
				$"The methods of type '{type.FullName}' should satisfy the required condition {typeFilter}.")));
	}

	/// <summary>
	///     The <see cref="Type" /> should satisfy the given <paramref name="constructorFilter" />.
	/// </summary>
	public static IRequirementResult<Type> Should(
		this IRequirement<Type> @this,
		IConstructorFilterResult constructorFilter)
	{
		Filter<Type> typeFilter = constructorFilter.ToTypeFilter();
		return @this.ShouldSatisfy(Requirement.ForType(
			typeFilter.Applies,
			type => new TypeTestError(type,
				$"The constructors of type '{type.FullName}' should satisfy the required condition {typeFilter}.")));
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
				$"Type '{type.FullName}' should satisfy the required condition {condition}.")));
	}
}
