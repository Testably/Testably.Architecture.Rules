using System;
using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     Add additional filters on the <see cref="FieldInfo" />s.
/// </summary>
public interface IFieldFilterResult : IFilter<FieldInfo>, IRequirement<FieldInfo>
{
	/// <summary>
	///     Add additional filters on the <see cref="FieldInfo" />s.
	/// </summary>
	IFieldFilter And { get; }

	/// <summary>
	///     Get all types from the filtered fields.
	/// </summary>
	ITypeExpectation Types { get; }
}
