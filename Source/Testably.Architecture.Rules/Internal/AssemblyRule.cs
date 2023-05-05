using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Testably.Architecture.Rules.Internal;

internal class AssemblyRule : Rule<Assembly>, IAssemblyExpectation
{
	/// <inheritdoc cref="IRule.Check" />
	public override IRuleCheck Check
		=> new RuleCheck<Assembly>(Filters, Requirements, Exemptions, _ => _);

	/// <inheritdoc cref="IAssemblyExpectation.Types" />
	public IFilter<Type> Types
		=> new TypeRule()
			.Which(new TypeAssemblyFilter(Filters)).And;

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
