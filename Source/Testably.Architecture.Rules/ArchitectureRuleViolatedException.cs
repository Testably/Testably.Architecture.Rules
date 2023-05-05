using System;
using System.Runtime.Serialization;

namespace Testably.Architecture.Rules;

/// <summary>
///     The architecture rule is violated.
/// </summary>
[Serializable]
public class ArchitectureRuleViolatedException : Exception
{
	/// <summary>
	///     Initializes a new instance of <see cref="ArchitectureRuleViolatedException" />.
	/// </summary>
	public ArchitectureRuleViolatedException(string message)
		: base(message)
	{
	}

	/// <summary>
	///     Initializes a new instance of <see cref="ArchitectureRuleViolatedException" />.
	/// </summary>
	/// <param name="info">
	///     The <see cref="SerializationInfo"></see> that holds the serialized object data
	///     about the exception being thrown.
	/// </param>
	/// <param name="context">
	///     The <see cref="StreamingContext"></see> that contains contextual information
	///     about the source or destination.
	/// </param>
	protected ArchitectureRuleViolatedException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
