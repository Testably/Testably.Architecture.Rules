using System;
using System.Collections.Generic;
using System.Linq;
using Testably.Architecture.Testing.TestErrors;

namespace Testably.Architecture.Testing.Filters;

/// <summary>
/// TODO VB
/// </summary>
public abstract class FilteredTypeExpectationOrBase : Filter<Type>, IFilteredTypeExpectation
{
	private readonly IExpectationFilter<Type> _filterableTypeExpectation;
	/// <summary>
	/// TODO VB
	/// </summary>
	protected readonly List<Func<Type, bool>> Predicates = new();
	private readonly IFilteredTypeExpectation _filtered;

	/// <summary>
	/// TODO VB
	/// </summary>
	/// <param name="filterableTypeExpectation"></param>
	/// <param name="predicate"></param>
	protected FilteredTypeExpectationOrBase(
		IExpectationFilter<Type> filterableTypeExpectation,
		Func<Type, bool> predicate)
	{
		_filterableTypeExpectation = filterableTypeExpectation;
		Predicates.Add(predicate);
		_filtered = _filterableTypeExpectation.Which(this);
	}

	/// <inheritdoc cref="Filter{T}.Applies(T)" />
	public override bool Applies(Type type)
		=> Predicates.Any(p => p(type));

	/// <inheritdoc cref="IExpectationCondition{Type}.ShouldSatisfy(Func{Type,bool}, Func{Type, TestError})" />
	public ITestResult<IExpectationCondition<Type>> ShouldSatisfy(Func<Type, bool> condition,
		Func<Type, TestError> errorGenerator)
		=> _filtered.ShouldSatisfy(condition, errorGenerator);

	/// <inheritdoc cref="IFilteredTypeExpectation.And" />
	public IExpectationFilter<Type> And => _filterableTypeExpectation;
}
