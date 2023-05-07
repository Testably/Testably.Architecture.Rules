using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Testably.Architecture.Rules.Internal;

internal class RuleCheck<TType> : IRuleCheck
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

	/// <inheritdoc cref="IRuleCheck.In(ITestDataProvider)" />
	public ITestResult In(ITestDataProvider testDataProvider)
	{
		List<TestError> errors = new();
		IEnumerable<Assembly> assemblies = testDataProvider.GetAssemblies();
		IEnumerable<TType> transformedSource = _transformer(assemblies);
		if (testDataProvider is IDataFilter<TType> dataFilter)
		{
			transformedSource = dataFilter.Filter(transformedSource);
		}
		List<TType> filteredSource = transformedSource
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
}
