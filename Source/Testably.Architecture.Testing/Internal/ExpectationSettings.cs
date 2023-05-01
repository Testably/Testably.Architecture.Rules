using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Testably.Architecture.Testing.Internal;

internal static class ExpectationSettings
{
	/// <summary>
	///     The list of <see cref="Assembly" />s to exclude from the current domain.
	/// </summary>
	public static readonly List<string> ExcludedSystemAssemblies = new()
	{
		"mscorlib",
		"System",
		"xunit"
	};

	public static bool IsExcluded(Assembly assembly)
		=> ExcludedSystemAssemblies.Any(
			excludedName => assembly.FullName?.StartsWith(
				excludedName,
				StringComparison.InvariantCulture) == true);
}
