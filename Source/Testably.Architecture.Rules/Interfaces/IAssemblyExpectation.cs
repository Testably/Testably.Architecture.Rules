using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     Defines expectations on <see cref="Assembly" />s that can be filtered.
/// </summary>
public interface IAssemblyExpectation : IAssemblyFilter, IRequirement<Assembly>
{
}
