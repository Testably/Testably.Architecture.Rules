using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Testably.Architecture.Rules.Internal;

internal class FieldRule : Rule<FieldInfo>, IFieldExpectation, IFieldFilterResult
{
	/// <inheritdoc cref="IRule.Check" />
	public override IRuleCheck Check
		=> new RuleCheck<FieldInfo>(Filters, Requirements, Exemptions,
			_ => _.SelectMany(a => a.GetTypes().SelectMany(t => t.GetFields())));

	public FieldRule(params Filter<FieldInfo>[] filters)
	{
		Filters.AddRange(filters);
	}

	#region IFieldExpectation Members

	/// <inheritdoc cref="IFieldFilter.Which(Filter{FieldInfo})" />
	public IFieldFilterResult Which(Filter<FieldInfo> filter)
	{
		Filters.Add(filter);
		return this;
	}

	#endregion

	#region IFieldFilterResult Members

	/// <inheritdoc cref="IFieldFilterResult.And" />
	public IFieldFilter And => this;

	/// <inheritdoc cref="IFieldFilterResult.Types" />
	public ITypeExpectation Types
		=> new TypeRule(new FieldTypeFilter(Filters));

	/// <inheritdoc cref="IFilter{FieldInfo}.Applies(FieldInfo)" />
	public bool Applies(FieldInfo type)
		=> Filters.All(f => f.Applies(type));

	#endregion

	/// <inheritdoc cref="object.ToString()" />
	public override string ToString()
		=> string.Join(" and ", Filters);

	private sealed class FieldTypeFilter : Filter<Type>
	{
		private readonly List<Filter<FieldInfo>> _fieldFilters;

		public FieldTypeFilter(List<Filter<FieldInfo>> fieldFilters)
		{
			_fieldFilters = fieldFilters;
		}

		/// <inheritdoc cref="Filter{Type}.Applies(Type)" />
		public override bool Applies(Type type)
		{
			return type.GetFields().Any(
				field => _fieldFilters.All(
					filter => filter.Applies(field)));
		}

		/// <inheritdoc cref="object.ToString()" />
		public override string ToString()
			=> $"The type must have a field {string.Join(" and ", _fieldFilters)}";
	}
}
