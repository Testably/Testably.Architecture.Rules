using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Testably.Architecture.Rules.Internal;

internal class RuleBundle : IRule, IRuleCheck
{
	private Action<string>? _logAction;
	private readonly string _name;
	private readonly IRule[] _rules;

	public RuleBundle(string name, IRule[] rules)
	{
		_name = name;
		_rules = rules;
	}

	#region IRule Members

	/// <inheritdoc cref="IRule.Check" />
	public IRuleCheck Check
		=> this;

	#endregion

	#region IRuleCheck Members

	/// <inheritdoc cref="IRuleCheck.In(ITestDataProvider)" />
	public ITestResult In(ITestDataProvider testDataProvider)
	{
		List<ITestResult> testResults = _rules
			.Select(rule => rule.Check.WithLog(_logAction).In(testDataProvider))
			.ToList();
		return new BundleTestResult(_name, testResults);
	}

	/// <inheritdoc cref="IRuleCheck.WithLog(Action{string})" />
	public IRuleCheck WithLog(Action<string>? logAction)
	{
		_logAction = logAction;
		return this;
	}

	#endregion

	private sealed class BundleTestResult : ITestResult
	{
		private string? _description;
		private readonly string _name;

		public BundleTestResult(string name, List<ITestResult> testResults)
		{
			_name = name;
			Errors = testResults.SelectMany(x => x.Errors).ToArray();
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
			sb.Append("The rules ");
			sb.Append('\'');
			sb.Append(_name);
			sb.Append("' ");
			if (!string.IsNullOrEmpty(_description))
			{
				sb.Append('(');
				sb.Append(_description);
				sb.Append(") ");
				return sb.ToString();
			}

			if (!IsViolated)
			{
				sb.Append("are not violated.");
				return sb.ToString();
			}

			sb.Append("are violated");
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
}
