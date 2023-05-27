using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Testably.Architecture.Rules.Internal;

internal class PropertyRule : Rule<PropertyInfo>, IPropertyExpectation, IPropertyFilterResult
{
	/// <inheritdoc cref="IRule.Check" />
	public override IRuleCheck Check
		=> new RuleCheck<PropertyInfo>(Filters, Requirements, Exemptions,
			_ => _.SelectMany(a => a.GetTypes().SelectMany(t => t.GetProperties())));

	public PropertyRule(params Filter<PropertyInfo>[] filters)
	{
		Filters.AddRange(filters);
	}

	#region IPropertyExpectation Members

	/// <inheritdoc cref="IPropertyFilter.Which(Filter{PropertyInfo})" />
	public IPropertyFilterResult Which(Filter<PropertyInfo> filter)
	{
		Filters.Add(filter);
		return this;
	}

	#endregion

	#region IPropertyFilterResult Members

	/// <inheritdoc cref="IPropertyFilterResult.And" />
	public IPropertyFilter And => this;

	/// <inheritdoc cref="IPropertyFilterResult.Types" />
	public ITypeExpectation Types
		=> new TypeRule(new PropertyTypeFilter(Filters));

	/// <inheritdoc cref="IFilter{PropertyInfo}.Applies(PropertyInfo)" />
	public bool Applies(PropertyInfo type)
		=> Filters.All(f => f.Applies(type));

	#endregion

	/// <inheritdoc cref="object.ToString()" />
	public override string ToString()
		=> string.Join(" and ", Filters);

	private sealed class PropertyTypeFilter : Filter<Type>
	{
		private readonly List<Filter<PropertyInfo>> _propertyFilters;

		public PropertyTypeFilter(List<Filter<PropertyInfo>> propertyFilters)
		{
			_propertyFilters = propertyFilters;
		}

		/// <inheritdoc cref="Filter{Type}.Applies(Type)" />
		public override bool Applies(Type type)
		{
			return type.GetProperties().Any(
				property => _propertyFilters.All(
					filter => filter.Applies(property)));
		}

		/// <inheritdoc cref="object.ToString()" />
		public override string ToString()
			=> $"The type must have a property which matches the filters: {string.Join(", ", _propertyFilters)}";
	}
}
