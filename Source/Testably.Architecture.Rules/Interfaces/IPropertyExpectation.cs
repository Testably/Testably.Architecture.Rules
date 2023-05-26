using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     Defines expectations on <see cref="PropertyInfo" />s that can be filtered.
/// </summary>
public interface IPropertyExpectation : IPropertyFilter, IRequirement<PropertyInfo>
{
}
