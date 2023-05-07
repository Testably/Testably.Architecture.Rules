using System;
using System.Collections.Generic;
using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     List of excluded <see cref="Assembly" />s and <see cref="Type" />s.
/// </summary>
public static class ExclusionLists
{
	/// <summary>
	///     The list of <see cref="Assembly" />s to exclude from rule checks.
	///     <para />
	///     All assemblies which <see cref="Assembly.FullName" /> starts with any of the
	///     excluded namespaces is omitted from the rule check.
	/// </summary>
	public static readonly List<string> ExcludedAssemblyNamespaces = new()
	{
		"mscorlib",
		"System",
		"xunit"
	};

	/// <summary>
	///     The list of <see cref="Type" />s to exclude from rule checks.
	///     <para />
	///     All types which <see cref="Type.FullName" /> starts with any of the
	///     excluded namespaces is omitted from the rule check.
	/// </summary>
	public static readonly List<string> ExcludedTypeNamespaces = new()
	{
		"System",
		"Microsoft",
		"xunit",
		"<Module>",
		"<PrivateImplementationDetails>"
	};
}
