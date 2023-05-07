using System;
using System.Collections.Generic;
using System.Text;

namespace Testably.Architecture.Rules.Internal;

internal class TestResult : ITestResult
{
	private string? _description;

	public TestResult(List<TestError> errors)
	{
		Errors = errors.ToArray();
	}

	#region ITestResult Members

	/// <inheritdoc cref="ITestResult.Errors" />
	public TestError[] Errors { get; }

	/// <inheritdoc cref="ITestResult.IsViolated" />
	public bool IsViolated => Errors.Length > 0;

	/// <inheritdoc cref="ITestResult.WithDescription(string)" />
	public ITestResult WithDescription(string description)
	{
		_description = description;
		return this;
	}

	#endregion

	/// <inheritdoc cref="object.ToString()" />
	public override string ToString()
	{
		StringBuilder sb = new();
		sb.Append("The rule ");
		if (!string.IsNullOrEmpty(_description))
		{
			sb.Append('\'');
			sb.Append(_description);
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
}
