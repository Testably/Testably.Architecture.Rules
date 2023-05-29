using System;
using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     Checks a <see cref="IRule" /> in given <see cref="Assembly" />s.
/// </summary>
public interface IRuleCheck
{
	/// <summary>
	///     The <see cref="ITestDataProvider" /> specifies in which assemblies the rule should be checked.
	/// </summary>
	ITestResult In(ITestDataProvider testDataProvider);

	/// <summary>
	///     (optional) If set, logs detailed information about the check steps of the <see cref="IRule" />.
	///     <para />
	///     This can be used for debugging rules.
	/// </summary>
	IRuleCheck WithLog(Action<string>? logAction);
}
