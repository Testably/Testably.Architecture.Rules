using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     Defines expectations on <see cref="FieldInfo" />s that can be filtered.
/// </summary>
public interface IFieldExpectation : IFieldFilter, IRequirement<FieldInfo>
{
}
