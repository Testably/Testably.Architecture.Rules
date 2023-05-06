using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Testably.Architecture.Rules.Internal;

internal class TypeRule : Rule<Type>, ITypeExpectation, ITypeFilterResult
{
	/// <inheritdoc cref="IRule.Check" />
	public override IRuleCheck Check
		=> new RuleCheck<Type>(Filters, Requirements, Exemptions,
			_ => _.SelectMany(a => a.GetTypes()));

	public TypeRule(params Filter<Type>[] filters)
	{
		Filters.AddRange(filters);
	}

	#region ITypeExpectation Members

	/// <inheritdoc cref="ITypeFilter.Which(Filter{Type})" />
	public ITypeFilterResult Which(Filter<Type> filter)
	{
		Filters.Add(filter);
		return this;
	}

	#endregion

	#region ITypeFilterResult Members

	/// <inheritdoc cref="ITypeFilterResult.And" />
	public ITypeFilter And => this;

	/// <inheritdoc />
	public IAssemblyExpectation Assemblies
		=> new AssemblyRule(new TypeAssemblyFilter(Filters));

	#endregion

	private sealed class TypeAssemblyFilter : Filter<Assembly>
	{
		private readonly List<Filter<Type>> _typeFilters;

		public TypeAssemblyFilter(List<Filter<Type>> typeFilters)
		{
			_typeFilters = typeFilters;
		}

		/// <inheritdoc cref="Filter{Assembly}.Applies(Assembly)" />
		public override bool Applies(Assembly assembly)
		{
			return assembly.GetTypes().Any(
				type => _typeFilters.All(
					filter => filter.Applies(type)));
		}

		/// <inheritdoc cref="object.ToString()" />
		public override string ToString()
			=> $"The assembly of the type must match the filters: {string.Join(", ", _typeFilters)}";
	}
}
