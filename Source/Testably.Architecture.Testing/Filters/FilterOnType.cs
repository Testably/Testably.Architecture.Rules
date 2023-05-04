using System;
using System.Collections.Generic;
using System.Linq;
using Testably.Architecture.Testing.TestErrors;

namespace Testably.Architecture.Testing.Filters;

/// <summary>
///     Base class for filters on <see cref="Type" />.
/// </summary>
public abstract partial class FilterOnType : Filter<Type>, IExpectationFilterResult<Type>
{
	/// <summary>
	///     The list of predicates.
	/// </summary>
	protected readonly List<Func<Type, bool>> Predicates = new();

	private readonly IExpectationFilter<Type> _expectationFilter;

	private readonly IExpectationFilterResult<Type> _filtered;

	/// <summary>
	///     Initializes a new instance of <see cref="FilterOnType" />.
	/// </summary>
	protected FilterOnType(
		IExpectationFilter<Type> expectationFilter,
		Func<Type, bool> predicate)
	{
		_expectationFilter = expectationFilter;
		Predicates.Add(predicate);
		_filtered = _expectationFilter.Which(this);
	}

	#region IExpectationFilterResult<Type> Members

	/// <inheritdoc cref="IExpectationFilterResult{Type}.And" />
	public IExpectationFilter<Type> And => _expectationFilter;

	/// <inheritdoc cref="IExpectationCondition{Type}.ShouldSatisfy(Func{Type,bool}, Func{Type, TestError})" />
	public IExpectationResult<Type> ShouldSatisfy(Func<Type, bool> condition,
		Func<Type, TestError> errorGenerator)
		=> _filtered.ShouldSatisfy(condition, errorGenerator);

	#endregion

	/// <inheritdoc cref="Filter{T}.Applies(T)" />
	public override bool Applies(Type type)
		=> Predicates.Any(p => p(type));
}
