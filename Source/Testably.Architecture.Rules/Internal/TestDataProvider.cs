using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Testably.Architecture.Rules.Internal;

internal class TestDataProvider : ITestDataProvider, IDataFilter<Assembly>, IDataFilter<Type>
{
	private readonly bool _applyExclusionFilters;
	private readonly IEnumerable<Assembly> _assemblies;

	internal TestDataProvider(IEnumerable<Assembly> assemblies,
		bool applyExclusionFilters = true)
	{
		_assemblies = assemblies;
		_applyExclusionFilters = applyExclusionFilters;
	}

	#region IDataFilter<Assembly> Members

	/// <inheritdoc cref="IDataFilter{Assembly}.Filter(IEnumerable{Assembly})" />
	public IEnumerable<Assembly> Filter(IEnumerable<Assembly> source)
	{
		if (!_applyExclusionFilters)
		{
			return source;
		}

		return source.Where(IncludeAssembly);
	}

	#endregion

	#region IDataFilter<Type> Members

	/// <inheritdoc cref="IDataFilter{Type}.Filter(IEnumerable{Type})" />
	public IEnumerable<Type> Filter(IEnumerable<Type> source)
	{
		if (!_applyExclusionFilters)
		{
			return source;
		}

		return source.Where(IncludeType);
	}

	#endregion

	#region ITestDataProvider Members

	/// <inheritdoc cref="ITestDataProvider.GetAssemblies()" />
	public IEnumerable<Assembly> GetAssemblies()
		=> Filter(_assemblies);

	#endregion

	private static bool IncludeAssembly(Assembly assembly)
		=> ExclusionLists.ExcludedAssemblyNamespaces.All(x
			=> assembly.FullName?.StartsWith(x, StringComparison.InvariantCulture) != true);

	private static bool IncludeType(Type type)
	{
		return ExclusionLists.ExcludedTypeNamespaces.All(x
			=> type.FullName?.StartsWith(x, StringComparison.InvariantCulture) != true);
	}
}
