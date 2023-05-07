using System;
using System.Collections.Generic;
using System.Text;

namespace Testably.Architecture.Rules.Internal;

internal class TestResult : ITestResult
{
	public TestResult(List<TestError> errors)
	{
		Errors = errors.ToArray();
	}

	#region ITestResult Members

	/// <inheritdoc cref="ITestResult.Errors" />
	public TestError[] Errors { get; }

	/// <inheritdoc cref="ITestResult.IsViolated" />
	public bool IsViolated => Errors.Length > 0;

	/// <inheritdoc />
	public string ToString(string ruleName)
	{
		StringBuilder sb = new();
		sb.Append("The rule ");
		if (!string.IsNullOrEmpty(ruleName))
		{
			sb.Append('\'');
			sb.Append(ruleName);
			sb.Append("' ");
		}

		if (!IsViolated)
		{
			sb.Append("is not violated.");
			return sb.ToString();
		}

		sb.Append("is violated");
		if (Errors.Length == 1)
		{
			sb.Append(':');
		}
		else
		{
			sb.Append(" with ");
			sb.Append(Errors.Length);
			sb.Append(" errors:");
		}

		sb.AppendLine();
		foreach (TestError error in Errors)
		{
			sb.Append(" - ");
			sb.AppendLine(
				error.ToString().Replace(Environment.NewLine, $"{Environment.NewLine}   "));
		}

		return sb.ToString();
	}

	#endregion

	/// <inheritdoc />
	public override string ToString()
		=> ToString("");
}
