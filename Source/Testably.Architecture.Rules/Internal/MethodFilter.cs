using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Testably.Architecture.Rules.Internal;

internal class MethodFilter : IMethodFilter, IMethodFilterResult
{
	private readonly List<Filter<MethodInfo>> _predicates = new();

	#region IMethodFilter Members

	/// <inheritdoc cref="IMethodFilter.Which(Filter{MethodInfo})" />
	public IMethodFilterResult Which(Filter<MethodInfo> filter)
	{
		_predicates.Add(filter);
		return this;
	}

	#endregion

	#region IMethodFilterResult Members

	/// <inheritdoc cref="IMethodFilterResult.And" />
	public IMethodFilter And
		=> this;

	/// <inheritdoc cref="IMethodFilterResult.ToTypeFilter()" />
	public Filter<Type> ToTypeFilter()
	{
		return Filter.FromPredicate<Type>(
			t => _predicates.All(p => t.GetMethods().Any(p.Applies)),
			ToString());
	}

	#endregion

	/// <inheritdoc cref="object.ToString()" />
	public override string ToString()
		=> string.Join(" and ", _predicates.Select(x => x.ToString()));
}
