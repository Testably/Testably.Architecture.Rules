using System;

namespace Testably.Architecture.Testing;

/// <summary>
/// Add additional filters on the <see cref="Type"/>s.
/// </summary>
public interface IFilteredTypeExpectation : ITypeExpectation
{
	/// <summary>
	/// Add additional filters on the <see cref="Type"/>s.
	/// </summary>
	IFilterableTypeExpectation And { get; }
}
