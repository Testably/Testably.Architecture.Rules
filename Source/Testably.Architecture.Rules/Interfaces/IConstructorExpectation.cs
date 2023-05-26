using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     Defines expectations on <see cref="ConstructorInfo" />s that can be filtered.
/// </summary>
public interface IConstructorExpectation : IConstructorFilter, IRequirement<ConstructorInfo>
{
}
