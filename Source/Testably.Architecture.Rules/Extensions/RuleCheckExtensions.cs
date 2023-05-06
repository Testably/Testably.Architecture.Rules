using System;
using System.Linq;
using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     Extension methods for <see cref="IRuleCheck" />.
/// </summary>
public static class RuleCheckExtensions
{
	/// <summary>
	///     Defines expectations on all loaded assemblies from the current <see cref="System.AppDomain.CurrentDomain" />.
	/// </summary>
	/// <param name="this">The <see cref="IRuleCheck" />.</param>
	/// <param name="predicate">(optional) A predicate to filter the assemblies.</param>
	/// <param name="excludeSystemAssemblies">
	///     Flag, indicating if system assemblies should be filtered out.
	///     <para />
	///     If set to <see langword="true" /> (default value), no assemblies starting with<br />
	///     - <c>mscorlib</c><br />
	///     - <c>System</c><br />
	///     - <c>xunit</c><br />
	///     are loaded.<br />
	///     Otherwise all assemblies matching the <paramref name="predicate" /> are loaded.
	/// </param>
	public static ITestResult InAllLoadedAssemblies(this IRuleCheck @this,
		Func<Assembly, bool>? predicate = null,
		bool excludeSystemAssemblies = true)
	{
		predicate ??= _ => true;
		return @this.In(
			AppDomain.CurrentDomain.GetAssemblies()
				.Where(predicate),
			excludeSystemAssemblies);
	}

	/// <summary>
	///     Defines expectations <see cref="InAllLoadedAssemblies(IRuleCheck, Func{Assembly,bool},bool)" />
	///     that match the <paramref name="pattern" />.
	/// </summary>
	/// <param name="this">The <see cref="IExpectation" />.</param>
	/// <param name="pattern">
	///     The wildcard condition.
	///     <para />
	///     Supports * to match zero or more characters and ? to match exactly one character.
	/// </param>
	/// <param name="ignoreCase">Flag indicating if the comparison should be case sensitive or not.</param>
	/// <param name="excludeSystemAssemblies">
	///     Flag, indicating if system assemblies should be filtered out.
	///     <para />
	///     If set to <see langword="true" /> (default value), no assemblies starting with<br />
	///     - <c>mscorlib</c><br />
	///     - <c>System</c><br />
	///     - <c>xunit</c><br />
	///     are loaded.<br />
	///     Otherwise all assemblies matching the <paramref name="pattern" /> are loaded.
	/// </param>
	public static ITestResult InAssembliesMatching(this IRuleCheck @this,
		Match pattern,
		bool ignoreCase = false,
		bool excludeSystemAssemblies = true)
	{
		return @this.InAllLoadedAssemblies(assembly
				=> pattern.Matches(assembly.GetName().Name, ignoreCase),
			excludeSystemAssemblies);
	}

	/// <summary>
	///     Defines expectations on the assembly that contains the <typeparamref name="TAssembly" />.
	/// </summary>
	public static ITestResult InAssemblyContaining<TAssembly>(this IRuleCheck @this)
	{
		return @this.In(new[]
		{
			typeof(TAssembly).Assembly
		});
	}

	/// <summary>
	///     Defines expectations on the <see cref="Assembly.GetExecutingAssembly()" />.
	/// </summary>
	public static ITestResult InExecutingAssembly(this IRuleCheck @this)
	{
		return @this.In(new[]
		{
			Assembly.GetExecutingAssembly()!
		});
	}

	/// <summary>
	///     Defines expectations on the given <paramref name="assemblies" />.
	/// </summary>
	public static ITestResult In(this IRuleCheck @this, params Assembly[] assemblies)
	{
		return @this.In(assemblies);
	}
}
