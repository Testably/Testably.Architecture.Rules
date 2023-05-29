using System.Collections.Generic;
using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     The test data provider specifies in which assemblies a <see cref="IRule" /> should be checked.
/// </summary>
public interface ITestDataProvider
{
	/// <summary>
	///     Gets the <see cref="Assembly" />s against which to check the <see cref="IRule" />s.
	/// </summary>
	/// <returns></returns>
	IEnumerable<Assembly> GetAssemblies();
}
