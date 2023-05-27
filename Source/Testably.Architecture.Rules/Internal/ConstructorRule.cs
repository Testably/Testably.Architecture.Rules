using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Testably.Architecture.Rules.Internal;

internal class ConstructorRule : Rule<ConstructorInfo>, IConstructorExpectation,
	IConstructorFilterResult
{
	/// <inheritdoc cref="IRule.Check" />
	public override IRuleCheck Check
		=> new RuleCheck<ConstructorInfo>(Filters, Requirements, Exemptions,
			_ => _.SelectMany(a => a.GetTypes().SelectMany(t => t.GetConstructors())));

	public ConstructorRule(params Filter<ConstructorInfo>[] filters)
	{
		Filters.AddRange(filters);
	}

	#region IConstructorExpectation Members

	/// <inheritdoc cref="IConstructorFilter.Which(Filter{ConstructorInfo})" />
	public IConstructorFilterResult Which(Filter<ConstructorInfo> filter)
	{
		Filters.Add(filter);
		return this;
	}

	#endregion

	#region IConstructorFilterResult Members

	/// <inheritdoc cref="IConstructorFilterResult.And" />
	public IConstructorFilter And => this;

	/// <inheritdoc cref="IConstructorFilterResult.Types" />
	public ITypeExpectation Types
		=> new TypeRule(new ConstructorTypeFilter(Filters));

	/// <inheritdoc cref="IFilter{ConstructorInfo}.Applies(ConstructorInfo)" />
	public bool Applies(ConstructorInfo type)
		=> Filters.All(f => f.Applies(type));

	#endregion

	/// <inheritdoc cref="object.ToString()" />
	public override string ToString()
		=> string.Join(" and ", Filters);

	private sealed class ConstructorTypeFilter : Filter<Type>
	{
		private readonly List<Filter<ConstructorInfo>> _constructorFilters;

		public ConstructorTypeFilter(List<Filter<ConstructorInfo>> constructorFilters)
		{
			_constructorFilters = constructorFilters;
		}

		/// <inheritdoc cref="Filter{Type}.Applies(Type)" />
		public override bool Applies(Type type)
		{
			return type.GetConstructors().Any(
				constructor => _constructorFilters.All(
					filter => filter.Applies(constructor)));
		}

		/// <inheritdoc cref="object.ToString()" />
		public override string ToString()
			=> $"The type must have a constructor which matches the filters: {string.Join(", ", _constructorFilters)}";
	}
}
