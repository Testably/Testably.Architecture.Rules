using System;

namespace Testably.Architecture.Testing;

/// <summary>
/// Add additional filters on the <see cref="Type"/>s.
/// </summary>
public interface IFilteredTypeExpectation : IExpectationCondition<Type>
{
	/// <summary>
	/// Add additional filters on the <see cref="Type"/>s.
	/// </summary>
	IExpectationFilter<Type> And { get; }
}
