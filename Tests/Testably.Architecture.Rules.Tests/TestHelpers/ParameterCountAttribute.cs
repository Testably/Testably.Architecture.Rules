using System;

namespace Testably.Architecture.Rules.Tests.TestHelpers;

[AttributeUsage(AttributeTargets.Constructor)]
public class ParameterCountAttribute : Attribute
{
	public int Count { get; }

	public ParameterCountAttribute(int count)
	{
		Count = count;
	}
}
