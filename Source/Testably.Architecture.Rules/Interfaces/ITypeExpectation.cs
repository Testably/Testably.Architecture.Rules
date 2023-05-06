using System;
using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     Defines expectations on <see cref="Type" />s that can be filtered.
/// </summary>
public interface ITypeExpectation : ITypeFilter, IRequirement<Type>
{
}
