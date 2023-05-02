using System;
using System.Collections.Generic;
using System.Linq;
using Testably.Architecture.Testing.TestErrors;

namespace Testably.Architecture.Testing.Filters;

public abstract class FilteredTypeExpectationOrBase : TypeFilter, IFilteredTypeExpectation
{
	private readonly IFilterableTypeExpectation _filterableTypeExpectation;
	protected readonly List<Func<Type, bool>> Predicates = new();
	private readonly IFilteredTypeExpectation _filtered;

	protected FilteredTypeExpectationOrBase(
		IFilterableTypeExpectation filterableTypeExpectation,
		Func<Type, bool> predicate)
	{
		_filterableTypeExpectation = filterableTypeExpectation;
		Predicates.Add(predicate);
		_filtered = _filterableTypeExpectation.Which(this);
	}

	/// <inheritdoc cref="TypeFilter.Applies(Type)" />
	public override bool Applies(Type type)
		=> Predicates.Any(p => p(type));

	/// <inheritdoc cref="ITypeExpectation.ShouldSatisfy(Func{Type,bool}, Func{Type, TestError})" />
	public ITestResult<ITypeExpectation> ShouldSatisfy(Func<Type, bool> condition,
		Func<Type, TestError> errorGenerator)
		=> _filtered.ShouldSatisfy(condition, errorGenerator);

	/// <inheritdoc cref="IFilteredTypeExpectation.And" />
	public IFilterableTypeExpectation And => _filterableTypeExpectation;
}
