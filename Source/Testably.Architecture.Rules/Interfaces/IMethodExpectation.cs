using System;
using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     Defines expectations on <see cref="MethodInfo" />s that can be filtered.
/// </summary>
public interface IMethodExpectation : IMethodFilter, IRequirement<MethodInfo>
{
}
