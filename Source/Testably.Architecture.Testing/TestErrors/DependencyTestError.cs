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
	///     The <see cref="Assembly" /> which has the incorrect dependency.
	/// </summary>
	public Assembly Project { get; }

	/// <summary>
	///     The <see cref="ProjectReferenceDefinition" />s that are incorrect in the <see cref="Project" />.
	/// </summary>
	public AssemblyName[] ProjectReferences
		=> _projectReferences.ToArray();

	private readonly List<AssemblyName> _projectReferences;

	/// <summary>
	///     Initializes a new instance of <see cref="DependencyTestError" />.
	/// </summary>
	/// <param name="project">The <see cref="Assembly" /> which has the incorrect dependency.</param>
	/// <param name="projectReferences">
	///     The <see cref="ProjectReferenceDefinition" />s that are incorrect in the
	///     <paramref name="project" />.
	/// </param>
	public DependencyTestError(Assembly project, AssemblyName[] projectReferences)
		: base(CreateMessage(project, projectReferences))
	{
		_projectReferences = projectReferences.ToList();
		Project = project;
	}

	internal bool Except(Func<Assembly, AssemblyName, bool> predicate)
	{
		_projectReferences.RemoveAll(r => predicate(Project, r));
		UpdateMessage(CreateMessage(Project, ProjectReferences));
		return _projectReferences.Count == 0;
	}

	/// <summary>
	///     Creates the message for the underlying <see cref="TestError" />.
	/// </summary>
	private static string CreateMessage(Assembly project,
	                                    AssemblyName[] projectReferences)
	{
		if (projectReferences.Length == 0)
		{
			return $"Project '{project.GetName().Name}' has no incorrect references.";
		}

		if (projectReferences.Length == 1)
		{
			return
				$"Project '{project.GetName().Name}' has an incorrect reference on '{projectReferences.Select(x => x.Name).Single()}'.";
		}

		List<string> references = projectReferences.Select(x => x.Name)
.Where(x => x != null).OrderBy(x => x).ToList()!;
		string lastReference = references.Last();
		references.Remove(lastReference);
		return
			$"Project '{project.GetName().Name}' has incorrect references on '{string.Join("', '", references)}' and '{lastReference}'.";
	}
}
