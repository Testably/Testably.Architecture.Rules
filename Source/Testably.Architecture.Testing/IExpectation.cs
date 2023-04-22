namespace Testably.Architecture.Testing;

/// <summary>
///     Definition for expectations on the architectural design.
/// </summary>
public interface IExpectation
{
	/// <summary>
	///     Defines expectations on all loaded projects from the current <see cref="System.AppDomain.CurrentDomain" />
	/// </summary>
	/// <returns></returns>
	IProjectExpectation AllLoadedProjects();

	/// <summary>
	///     Defines expectations on the project from the assembly that contains the <typeparamref name="TAssembly" />.
	/// </summary>
	IProjectExpectation ProjectContaining<TAssembly>();
}
