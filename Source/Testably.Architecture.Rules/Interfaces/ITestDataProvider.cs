using System.Collections.Generic;
using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     The provider for test data.
/// </summary>
public interface ITestDataProvider
{
	/// <summary>
	///     Gets the <see cref="Assembly" />s against which to check the rules.
	/// </summary>
	/// <returns></returns>
	IEnumerable<Assembly> GetAssemblies();
}
