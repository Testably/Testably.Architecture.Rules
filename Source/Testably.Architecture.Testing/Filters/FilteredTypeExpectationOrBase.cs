using System;
using System.Collections.Generic;
using System.Linq;
using Testably.Architecture.Testing.TestErrors;

namespace Testably.Architecture.Testing.Filters;

public abstract class FilteredTypeExpectationOrBase : TypeFilter, IFilteredTypeExpectation
{
	private readonly IFilterableTypeExpectation _this;
	protected readonly List<Func<Type, bool>> Predicates = new();

	protected FilteredTypeExpectationOrBase(
		IFilterableTypeExpectation @this,
		Func<Type, bool> predicate)
	{
		_this = @this;
		Predicates.Add(predicate);
	}

	/// <inheritdoc cref="TypeFilter.Applies(Type)" />
	public override bool Applies(Type type)
		=> Predicates.Any(p => p(type));

	/// <inheritdoc cref="ITypeExpectation.ShouldSatisfy(Func{Type,bool}, Func{Type, TestError})" />
	public ITestResult<ITypeExpectation> ShouldSatisfy(Func<Type, bool> condition,
		Func<Type, TestError> errorGenerator)
		=> _this.ShouldSatisfy(condition, errorGenerator);

	/// <inheritdoc cref="IFilteredTypeExpectation.And" />
	public IFilterableTypeExpectation And => _this;
}
