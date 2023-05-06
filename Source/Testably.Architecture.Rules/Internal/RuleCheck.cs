using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Testably.Architecture.Rules.Internal;

internal class RuleCheck<TType> : RuleCheck, IRuleCheck
{
	private readonly List<Exemption> _exemptions;
	private readonly List<Filter<TType>> _filters;
	private readonly List<Requirement<TType>> _requirements;
	private readonly Func<IEnumerable<Assembly>, IEnumerable<TType>> _transformer;

	public RuleCheck(List<Filter<TType>> filters,
		List<Requirement<TType>> requirements,
		List<Exemption> exemptions,
		Func<IEnumerable<Assembly>, IEnumerable<TType>> transformer)
	{
		_filters = filters;
		_requirements = requirements;
		_exemptions = exemptions;
		_transformer = transformer;
	}

	#region IRuleCheck Members

	/// <inheritdoc cref="IRuleCheck.In(IEnumerable{Assembly},bool)" />
	public ITestResult In(IEnumerable<Assembly> assemblies, bool excludeSystemAssemblies = true)
	{
		List<TestError> errors = new();
		IEnumerable<Assembly> filteredAssemblies =
			FilterOutSystemAssemblies(assemblies, excludeSystemAssemblies);
		List<TType> filteredSource = _transformer(filteredAssemblies)
			.Where(assembly => _filters.All(filter => filter.Applies(assembly)))
			.ToList();
		if (filteredSource.Count == 0)
		{
			if (_filters.Count == 1)
			{
				errors.Add(new EmptySourceTestError(
					$"No {typeof(TType).Name} was found that matches the filter: {_filters.Single()}"));
			}
			else
			{
				errors.Add(new EmptySourceTestError(
					$"No {typeof(TType).Name} was found that matches the {_filters.Count} filters:\n - {string.Join("\n - ", _filters)}"));
			}
		}

		foreach (TType item in filteredSource)
		{
			foreach (Requirement<TType> requirement in _requirements)
			{
				requirement.CollectErrors(item, errors);
			}
		}

		errors.RemoveAll(error =>
			_exemptions.Any(exemption => exemption.Exempt(error)));

		return new TestResult(errors);
	}

	#endregion

	private IEnumerable<Assembly> FilterOutSystemAssemblies(IEnumerable<Assembly> assemblies,
		bool excludeSystemAssemblies)
	{
		if (!excludeSystemAssemblies)
		{
			return assemblies;
		}

		return assemblies
			.Where(assembly =>
				!excludeSystemAssemblies ||
				!IsSystemAssembly(assembly));
	}

	private static bool IsSystemAssembly(Assembly assembly)
		=> ExcludedSystemAssemblies.Any(
			excludedName => assembly.FullName?.StartsWith(
				excludedName,
				StringComparison.InvariantCulture) == true);
}

internal class RuleCheck
{
	/// <summary>
	///     The list of <see cref="Assembly" />s to exclude from the current domain.
	/// </summary>
	protected static readonly List<string> ExcludedSystemAssemblies = new()
	{
		"mscorlib",
		"System",
		"xunit"
	};
}
