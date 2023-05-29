using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     An <see cref="AssemblyTestError" /> due to an incorrect dependency.
/// </summary>
public class DependencyTestError : AssemblyTestError
{
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
		: base(assembly, CreateMessage(assembly, assemblyReferences))
	{
		_referencedAssemblies = assemblyReferences.ToList();
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
		if (assemblyReferences.Length > 1)
		{
			List<string> references = assemblyReferences
				.Select(x => x.Name)
				.Where(x => x != null)
				.OrderBy(x => x)
				.ToList()!;
			string lastReference = references.Last();
			references.Remove(lastReference);
			return
				$"Assembly '{assembly.GetName().Name}' has {assemblyReferences.Length} incorrect references on '{string.Join("', '", references)}' and '{lastReference}'.";
		}

		string? assemblyReference = assemblyReferences
			.Select(x => x.Name)
			.SingleOrDefault();
		if (assemblyReference != null)
		{
			return
				$"Assembly '{assembly.GetName().Name}' has an incorrect reference on '{assemblyReference}'.";
		}

		return $"Assembly '{assembly.GetName().Name}' has no incorrect references.";
	}
}
