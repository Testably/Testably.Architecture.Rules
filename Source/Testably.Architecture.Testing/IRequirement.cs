using System;

namespace Testably.Architecture.Testing;

/// <summary>
///     Defines expectations on <typeparamref name="TType" />.
/// </summary>
public interface IRequirement<TType>
{
	/// <summary>
	///     The <typeparamref name="TType" /> should satisfy the given <paramref name="condition" />.
	/// </summary>
	IRequirementResult<TType> ShouldSatisfy(
		Func<TType, bool> condition,
		Func<TType, TestError> errorGenerator);
}
