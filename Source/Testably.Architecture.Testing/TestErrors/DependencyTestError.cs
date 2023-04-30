using System;
using System.Collections.Generic;
using System.Linq;
using Testably.Architecture.Testing.Models;

namespace Testably.Architecture.Testing.TestErrors;

/// <summary>
///     A <see cref="TestError" /> due to an incorrect dependency.
/// </summary>
public class DependencyTestError : TestError
{
	/// <summary>
	///     The <see cref="Models.Project" /> which has the incorrect dependency.
	/// </summary>
	public Project Project { get; }

	/// <summary>
	///     The <see cref="ProjectReference" />s that are incorrect in the <see cref="Project" />.
	/// </summary>
	public ProjectReference[] ProjectReferences
		=> _projectReferences.ToArray();

	private readonly List<ProjectReference> _projectReferences;

	/// <summary>
	///     Initializes a new instance of <see cref="DependencyTestError" />.
	/// </summary>
	/// <param name="project">The <see cref="Models.Project" /> which has the incorrect dependency.</param>
	/// <param name="projectReferences">
	///     The <see cref="ProjectReference" />s that are incorrect in the
	///     <paramref name="project" />.
	/// </param>
	public DependencyTestError(Project project, ProjectReference[] projectReferences)
		: base(CreateMessage(project, projectReferences))
	{
		_projectReferences = projectReferences.ToList();
		Project = project;
	}

	internal bool Except(Func<Project, ProjectReference, bool> predicate)
	{
		_projectReferences.RemoveAll(r => predicate(Project, r));
		UpdateMessage(CreateMessage(Project, ProjectReferences));
		return _projectReferences.Count == 0;
	}

	/// <summary>
	///     Creates the message for the underlying <see cref="TestError" />.
	/// </summary>
	private static string CreateMessage(Project project, ProjectReference[] projectReferences)
	{
		if (projectReferences.Length == 0)
		{
			return $"Project '{project.Name}' has no incorrect references.";
		}

		if (projectReferences.Length == 1)
		{
			return
				$"Project '{project.Name}' has an incorrect reference on '{projectReferences.Select(x => x.Name).Single()}'.";
		}

		List<string> references = projectReferences.Select(x => x.Name).OrderBy(x => x).ToList();
		string lastReference = references.Last();
		references.Remove(lastReference);
		return
			$"Project '{project.Name}' has incorrect references on '{string.Join("', '", references)}' and '{lastReference}'.";
	}
}
