using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     Add additional filters on the <see cref="PropertyInfo" />s.
/// </summary>
public interface IPropertyFilterResult : IFilter<PropertyInfo>, IRequirement<PropertyInfo>
{
	/// <summary>
	///     Add additional filters on the <see cref="PropertyInfo" />s.
	/// </summary>
	IPropertyFilter And { get; }

	/// <summary>
	///     Get all types from the filtered properties.
	/// </summary>
	ITypeExpectation Types { get; }
}
