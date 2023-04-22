using System;

namespace Testably.Architecture.Testing;

/// <summary>
///     Extension methods for <see cref="IExpectation" />.
/// </summary>
public static class ExpectationExtensions
{
	/// <summary>
	///     Defines expectations on all loaded projects from the current <see cref="System.AppDomain.CurrentDomain" />
	/// </summary>
	public static IProjectExpectation AllLoadedProjects(this IExpectation @this)
	{
		return @this.FromAssembly(AppDomain.CurrentDomain.GetAssemblies());
	}

	/// <summary>
	///     Defines expectations on the project from the assembly that contains the <typeparamref name="TAssembly" />.
	/// </summary>
	public static IProjectExpectation ProjectContaining<TAssembly>(this IExpectation @this)
	{
		return @this.FromAssembly(typeof(TAssembly).Assembly);
	}
}
