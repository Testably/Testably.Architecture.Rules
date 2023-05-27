using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Testably.Architecture.Rules.Internal;

internal class MethodRule : Rule<MethodInfo>, IMethodExpectation, IMethodFilterResult
{
	/// <inheritdoc cref="IRule.Check" />
	public override IRuleCheck Check
		=> new RuleCheck<MethodInfo>(Filters, Requirements, Exemptions,
			assemblies => assemblies
				.SelectMany(assembly => assembly.GetTypes()
					.SelectMany(type => type.GetDeclaredMethods())));

	public MethodRule(params Filter<MethodInfo>[] filters)
	{
		Filters.AddRange(filters);
	}

	#region IMethodExpectation Members

	/// <inheritdoc cref="IMethodFilter.Which(Filter{MethodInfo})" />
	public IMethodFilterResult Which(Filter<MethodInfo> filter)
	{
		Filters.Add(filter);
		return this;
	}

	#endregion

	#region IMethodFilterResult Members

	/// <inheritdoc cref="IMethodFilterResult.And" />
	public IMethodFilter And => this;

	/// <inheritdoc cref="IMethodFilterResult.Types" />
	public ITypeExpectation Types
		=> new TypeRule(new MethodTypeFilter(Filters));

	/// <inheritdoc cref="IFilter{MethodInfo}.Applies(MethodInfo)" />
	public bool Applies(MethodInfo type)
		=> Filters.All(f => f.Applies(type));

	#endregion

	/// <inheritdoc cref="object.ToString()" />
	public override string ToString()
		=> string.Join(" and ", Filters);

	private sealed class MethodTypeFilter : Filter<Type>
	{
		private readonly List<Filter<MethodInfo>> _methodFilters;

		public MethodTypeFilter(List<Filter<MethodInfo>> methodFilters)
		{
			_methodFilters = methodFilters;
		}

		/// <inheritdoc cref="Filter{Type}.Applies(Type)" />
		public override bool Applies(Type type)
		{
			return type.GetDeclaredMethods().Any(
				method => _methodFilters.All(
					filter => filter.Applies(method)));
		}

		/// <inheritdoc cref="object.ToString()" />
		public override string ToString()
			=> $"The type must have a method {string.Join(" and ", _methodFilters)}";
	}
}
