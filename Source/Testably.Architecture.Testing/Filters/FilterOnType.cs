using System;
using System.Collections.Generic;
using System.Linq;

namespace Testably.Architecture.Testing;

/// <summary>
///     Base class for filters on <see cref="Type" />.
/// </summary>
public abstract partial class FilterOnType : Filter<Type>, IFilterResult<Type>
{
	/// <summary>
	///     The list of predicates.
	/// </summary>
	protected readonly List<Func<Type, bool>> Predicates = new();

	private readonly IFilter<Type> _expectationFilter;

	private readonly IFilterResult<Type> _filtered;

	/// <summary>
	///     Initializes a new instance of <see cref="FilterOnType" />.
	/// </summary>
	protected FilterOnType(
		IFilter<Type> expectationFilter,
		Func<Type, bool> predicate)
	{
		_expectationFilter = expectationFilter;
		Predicates.Add(predicate);
		_filtered = _expectationFilter.Which(this);
	}

	#region IFilterResult<Type> Members

	/// <inheritdoc cref="IFilterResult{Type}.And" />
	public IFilter<Type> And => _expectationFilter;

	/// <inheritdoc cref="IRequirement{Type}.ShouldSatisfy(Func{Type,bool}, Func{Type, TestError})" />
	public IRequirementResult<Type> ShouldSatisfy(Func<Type, bool> condition,
		Func<Type, TestError> errorGenerator)
		=> _filtered.ShouldSatisfy(condition, errorGenerator);

	#endregion

	/// <inheritdoc cref="Filter{T}.Applies(T)" />
	public override bool Applies(Type type)
		=> Predicates.Any(p => p(type));
}
