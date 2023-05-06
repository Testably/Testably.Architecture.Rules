using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     Defines expectations on <see cref="Assembly" /> that can be filtered.
/// </summary>
public interface IAssemblyFilter
{
	/// <summary>
	///     Filters the applicable <see cref="Assembly" /> on which the expectations should be applied.
	/// </summary>
	/// <param name="filter">The filter to apply on the <see cref="Assembly" />.</param>
	IAssemblyFilterResult Which(Filter<Assembly> filter);
}
