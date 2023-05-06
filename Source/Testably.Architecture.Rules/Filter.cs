using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Testably.Architecture.Rules;

/// <summary>
///     Filters to apply in the architecture rule.
/// </summary>
public static class Filter
{
	/// <summary>
	///     Creates a new <see cref="Filter{TType}" /> from the given <paramref name="predicate" />.
	/// </summary>
	public static Filter<TType> FromPredicate<TType>(Expression<Func<TType, bool>> predicate)
	{
		Func<TType, bool> compiledPredicate = predicate.Compile();
		return new GenericFilter<TType>(compiledPredicate, predicate.ToString());
	}

	/// <summary>
	///     Creates a new <see cref="Filter{TType}" /> from the given <paramref name="predicate" />.
	/// </summary>
	public static Filter<TType> FromPredicate<TType>(Func<TType, bool> predicate, string name)
		=> new GenericFilter<TType>(predicate, name);

	private sealed class GenericFilter<TType> : Filter<TType>
	{
		private readonly Func<TType, bool> _filter;
		private readonly string _name;

		public GenericFilter(Func<TType, bool> filter, string name)
		{
			_filter = filter;
			_name = name;
		}

		/// <inheritdoc cref="Filter{TType}.Applies(TType)" />
		public override bool Applies(TType type)
			=> _filter(type);

		/// <inheritdoc cref="object.ToString()" />
		public override string ToString()
			=> _name;
	}

	/// <summary>
	///     Base class for additional filters on <see cref="Type" />.
	/// </summary>
	public abstract class OnType : Filter<Type>, ITypeFilterResult
	{
		/// <summary>
		///     The list of predicates.
		/// </summary>
		protected readonly List<Func<Type, bool>> Predicates = new();

		private readonly ITypeFilter _typeFilter;

		private readonly ITypeFilterResult _filtered;

		/// <summary>
		///     Initializes a new instance of <see cref="OnType" />.
		/// </summary>
		protected OnType(
			ITypeFilter typeFilter,
			Func<Type, bool> predicate)
		{
			_typeFilter = typeFilter;
			Predicates.Add(predicate);
			_filtered = _typeFilter.Which(this);
		}

		#region ITypeFilterResult Members

		/// <inheritdoc cref="ITypeFilterResult.And" />
		public ITypeFilter And => _typeFilter;

		/// <inheritdoc cref="ITypeFilterResult.Assemblies" />
		public IAssemblyExpectation Assemblies
			=> _filtered.Assemblies;

		/// <inheritdoc cref="IRequirement{Type}.ShouldSatisfy(Requirement{Type})" />
		public IRequirementResult<Type> ShouldSatisfy(Requirement<Type> requirement)
			=> _filtered.ShouldSatisfy(requirement);

		#endregion

		/// <inheritdoc cref="Filter{T}.Applies(T)" />
		public override bool Applies(Type type)
			=> Predicates.Any(p => p(type));
	}
}

/// <summary>
///     Filter for <typeparamref name="TType" />.
/// </summary>
public abstract class Filter<TType>
{
	/// <summary>
	///     Specifies if the filter applies to the given <typeparamref name="TType" />.
	/// </summary>
	public abstract bool Applies(TType type);
}
