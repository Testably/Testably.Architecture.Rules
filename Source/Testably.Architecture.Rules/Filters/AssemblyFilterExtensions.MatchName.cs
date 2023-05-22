using System.Reflection;

namespace Testably.Architecture.Rules;

public static partial class AssemblyFilterExtensions
{
	/// <summary>
	///     Filter for assemblies where the <see cref="AssemblyName.Name" /> matches the given <paramref name="pattern" />.
	/// </summary>
	public static IAssemblyFilterResult WhichMatchName(this IAssemblyFilter @this,
		Match pattern,
		bool ignoreCase = false)
	{
		return @this.Which(assembly => pattern.Matches(assembly.GetName().Name, ignoreCase));
	}

	/// <summary>
	///     Filter for assemblies where the <see cref="AssemblyName.Name" /> does not match the given <paramref name="pattern" />.
	/// </summary>
	public static IAssemblyFilterResult WhichDoNotMatchName(this IAssemblyFilter @this,
		Match pattern,
		bool ignoreCase = false)
	{
		return @this.Which(assembly => !pattern.Matches(assembly.GetName().Name, ignoreCase));
	}
}
