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
	private Action<string>? _logAction;

	public RuleCheck(
		List<Filter<TType>> filters,
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

		List<TType> transformedSourceList = transformedSource.ToList();
		_logAction.Log(
			$"Found {transformedSourceList.Count} {typeof(TType).Name}s before applying {_filters.Count} filters:");
		foreach (Filter<TType> filter in _filters)
		{
			_logAction.Log("  Apply filter " + filter);
		}

		List<TType> filteredSource = transformedSourceList
			.Where(assembly => _filters.All(filter => filter.Applies(assembly)))
			.ToList();
		_logAction.Log(
			$"Found {filteredSource.Count} {typeof(TType).Name}s after applying {_filters.Count} filters.");
		if (filteredSource.Count == 0)
		{
			if (_filters.Count == 1)
			{
				errors.Add(new EmptySourceTestError(
					$"No {typeof(TType).Name} was found that matches the filter: {_filters[0]}"));
			}
			else
			{
				errors.Add(new EmptySourceTestError(
					$"No {typeof(TType).Name} was found that matches the {_filters.Count} filters:{Environment.NewLine} - {string.Join($"{Environment.NewLine} - ", _filters)}"));
			}
		}

		foreach (TType item in filteredSource)
		{
			List<TestError> newErrors = new();
			foreach (Requirement<TType> requirement in _requirements)
			{
				requirement.CollectErrors(item, newErrors);
			}

			_logAction.Log($"Found {newErrors.Count} errors in {typeof(TType).Name} {item}.");
			foreach (TestError error in newErrors)
			{
				_logAction.Log(
					$"- {error.ToString().Replace(Environment.NewLine, $"{Environment.NewLine}  ")}");
			}

			errors.AddRange(newErrors);
		}

		_logAction.Log($"Found {errors.Count} total errors.");
		foreach (Exemption exemption in _exemptions)
		{
			_logAction.Log("  Apply exemption " + exemption);
		}

		errors.RemoveAll(error =>
			_exemptions.Any(exemption => exemption.Exempt(error)));
		_logAction.Log(
			$"After applying {_exemptions.Count} exemptions, {errors.Count} errors remain.");

		return new TestResult(errors);
	}

	/// <inheritdoc cref="IRuleCheck.WithLog(Action{string})" />
	public IRuleCheck WithLog(Action<string>? logAction)
	{
		_logAction = logAction;
		return this;
	}

	#endregion
}
