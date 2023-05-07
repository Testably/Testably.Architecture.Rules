using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Testably.Architecture.Rules.Internal;

internal interface IDataFilter<TData>
{
	IEnumerable<TData> Filter(IEnumerable<TData> source);
}

internal class TestDataProvider : ITestDataProvider, IDataFilter<Assembly>, IDataFilter<Type>
{
	private readonly IEnumerable<Assembly> _assemblies;
	private readonly bool _excludeSystemAssemblies;

	public TestDataProvider(IEnumerable<Assembly> assemblies,
		bool excludeSystemAssemblies = true)
	{
		_assemblies = assemblies;
		_excludeSystemAssemblies = excludeSystemAssemblies;
	}

	/// <inheritdoc cref="ITestDataProvider.GetAssemblies()" />
	public IEnumerable<Assembly> GetAssemblies()
		=> _assemblies;

	/// <inheritdoc cref="IDataFilter{Assembly}.Filter(IEnumerable{Assembly})"/>
	public IEnumerable<Assembly> Filter(IEnumerable<Assembly> source)
	{
		if (!_excludeSystemAssemblies)
		{
			return source;
		}

		return source
			.Where(assembly =>
				!IsSystemAssembly(assembly));
	}

	/// <inheritdoc cref="IDataFilter{Type}.Filter(IEnumerable{Type})"/>
	public IEnumerable<Type> Filter(IEnumerable<Type> source)
	{
		if (!_excludeSystemAssemblies)
		{
			return source;
		}

		return source
			.Where(type => type.FullName?.StartsWith("Testably") == true);
	}
	/// <summary>
	///     The list of <see cref="Assembly" />s to exclude from the current domain.
	/// </summary>
	public static readonly List<string> ExcludedSystemAssemblies = new()
	{
		"mscorlib",
		"System",
		"xunit"
	};

	private static bool IsSystemAssembly(Assembly assembly)
		=> ExcludedSystemAssemblies.Any(
			excludedName => assembly.FullName?.StartsWith(
				excludedName,
				StringComparison.InvariantCulture) == true);
}
