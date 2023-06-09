﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Testably.Architecture.Rules.Internal;

internal class AssemblyRule : Rule<Assembly>, IAssemblyExpectation, IAssemblyFilterResult
{
	/// <inheritdoc cref="IRule.Check" />
	public override IRuleCheck Check
		=> new RuleCheck<Assembly>(Filters, Requirements, Exemptions, _ => _);

	public AssemblyRule(params Filter<Assembly>[] filters)
	{
		Filters.AddRange(filters);
	}

	#region IAssemblyExpectation Members

	/// <inheritdoc cref="IAssemblyFilter.Which(Filter{Assembly})" />
	public IAssemblyFilterResult Which(Filter<Assembly> filter)
	{
		Filters.Add(filter);
		return this;
	}

	#endregion

	#region IAssemblyFilterResult Members

	/// <inheritdoc cref="IAssemblyFilterResult.And" />
	public IAssemblyFilter And => this;

	/// <inheritdoc cref="IAssemblyFilterResult.Types" />
	public ITypeExpectation Types
		=> new TypeRule(new TypeAssemblyFilter(Filters));

	#endregion

	/// <inheritdoc cref="object.ToString()" />
	public override string ToString()
		=> string.Join(" and ", Filters);

	private sealed class TypeAssemblyFilter : Filter<Type>
	{
		private readonly List<Filter<Assembly>> _assemblyFilters;

		public TypeAssemblyFilter(List<Filter<Assembly>> assemblyFilters)
		{
			_assemblyFilters = assemblyFilters;
		}

		/// <inheritdoc cref="Filter{Type}.Applies(Type)" />
		public override bool Applies(Type type)
		{
			return _assemblyFilters.All(filter => filter.Applies(type.Assembly));
		}

		/// <inheritdoc cref="object.ToString()" />
		public override string ToString()
			=> $"The assembly of the type must match the filters: {string.Join(", ", _assemblyFilters)}";
	}
}
