using System;
using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     Allows checking a <see cref="IRule" /> in given <see cref="Assembly" />s.
/// </summary>
public interface IRuleCheck
{
	/// <summary>
	///     Specifies in <see cref="ITestDataProvider" /> which provides the assemblies
	///     in which the rule should be checked.
	/// </summary>
	ITestResult In(ITestDataProvider testDataProvider);

	/// <summary>
	///     Log detailed information about the check steps of the <see cref="IRule" />.
	/// </summary>
	IRuleCheck WithLog(Action<string>? logAction);
}
