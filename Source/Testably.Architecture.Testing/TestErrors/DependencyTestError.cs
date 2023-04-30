using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Testably.Architecture.Testing.TestErrors;

/// <summary>
///     A <see cref="TestError" /> due to an incorrect dependency.
/// </summary>
public class DependencyTestError : TestError
{
	/// <summary>
	///     The <see cref="System.Reflection.Assembly" /> which has the incorrect dependency.
	/// </summary>
	public Assembly Assembly { get; }

	/// <summary>
	///     The <see cref="Assembly.GetReferencedAssemblies()" /> that are incorrect.
	/// </summary>
	public AssemblyName[] AssemblyReferences
		=> _referencedAssemblies.ToArray();

	private readonly List<AssemblyName> _referencedAssemblies;

	/// <summary>
	///     Initializes a new instance of <see cref="DependencyTestError" />.
	/// </summary>
	/// <param name="assembly">The <see cref="System.Reflection.Assembly" /> which has the incorrect dependency.</param>
	/// <param name="assemblyReferences">
	///     The <see cref="Assembly.GetReferencedAssemblies()" /> that are incorrect in the <paramref name="assembly" />.
	/// </param>
	public DependencyTestError(Assembly assembly, AssemblyName[] assemblyReferences)
		: base(CreateMessage(assembly, assemblyReferences))
	{
		_referencedAssemblies = assemblyReferences.ToList();
		Assembly = assembly;
	}

	internal bool Except(Func<Assembly, AssemblyName, bool> predicate)
	{
		_referencedAssemblies.RemoveAll(r => predicate(Assembly, r));
		UpdateMessage(CreateMessage(Assembly, AssemblyReferences));
		return _referencedAssemblies.Count == 0;
	}

	/// <summary>
	///     Creates the message for the underlying <see cref="TestError" />.
	/// </summary>
	private static string CreateMessage(Assembly assembly,
		AssemblyName[] assemblyReferences)
	{
		if (assemblyReferences.Length == 0)
		{
			return $"Assembly '{assembly.GetName().Name}' has no incorrect references.";
		}

		if (assemblyReferences.Length == 1)
		{
			return
				$"Assembly '{assembly.GetName().Name}' has an incorrect reference on '{assemblyReferences.Select(x => x.Name).Single()}'.";
		}

		List<string> references = assemblyReferences.Select(x => x.Name)
			.Where(x => x != null).OrderBy(x => x).ToList()!;
		string lastReference = references.Last();
		references.Remove(lastReference);
		return
			$"Assembly '{assembly.GetName().Name}' has incorrect references on '{string.Join("', '", references)}' and '{lastReference}'.";
	}
}
