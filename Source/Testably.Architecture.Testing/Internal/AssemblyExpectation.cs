using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Testably.Architecture.Testing.Internal;

internal class AssemblyExpectationStart : IAssemblyExpectation, IFilterResult<Assembly>
{
	private bool _allowEmpty;
	private readonly List<Filter<Assembly>> _filters = new();
	private readonly TestResultBuilder<Assembly> _testResultBuilder;
	private readonly List<Assembly> _types;

	public AssemblyExpectationStart(IEnumerable<Assembly> types)
	{
		_types = types.ToList();
		_testResultBuilder = new TestResultBuilder<Assembly>(this);
	}

	#region IAssemblyExpectation Members

	/// <inheritdoc />
	public ITypeExpectation Types
	{
		get
		{
			return new TypeExpectationStart(_types.SelectMany(x => x.GetTypes()));
		}
	}

	/// <inheritdoc />
	public IExpectationStart<Assembly> OrNone()
	{
		_allowEmpty = true;
		return this;
	}

	/// <inheritdoc cref="IFilter{Assembly}.Which(Filter{Assembly})" />
	public IFilterResult<Assembly> Which(Filter<Assembly> filter)
	{
		_filters.Add(filter);
		return this;
	}

	#pragma warning disable CS1574
	/// <inheritdoc cref="IFilter.ShouldSatisfy(Func{Assembly, bool}, Func{Assembly, TestError})" />
	#pragma warning restore CS1574
	public IRequirementResult<Assembly> ShouldSatisfy(
		Func<Assembly, bool> condition,
		Func<Assembly, TestError> errorGenerator)
	{
		List<Assembly>? types = _types.Where(x => _filters.All(f => f.Applies(x))).ToList();
		if (types.Count == 0 && !_allowEmpty)
		{
			throw new EmptyDataException(
				$"No assemblies found, that match all {_filters.Count} filters.");
		}

		foreach (Assembly type in types)
		{
			if (!condition(type))
			{
				TestError error = errorGenerator(type);
				_testResultBuilder.Add(error);
			}
		}

		return _testResultBuilder.Build();
	}

	#endregion

	#region IFilterResult<Assembly> Members

	/// <inheritdoc cref="IFilterResult{Assembly}.And" />
	public IFilter<Assembly> And => this;

	#endregion
}
