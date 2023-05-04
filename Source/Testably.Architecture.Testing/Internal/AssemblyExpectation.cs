using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Testably.Architecture.Testing.Exceptions;
using Testably.Architecture.Testing.TestErrors;

namespace Testably.Architecture.Testing.Internal;

internal class AssemblyExpectationStart : IAssemblyExpectation, IExpectationFilterResult<Assembly>
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

	#pragma warning disable CS1574
	/// <inheritdoc cref="IExpectationFilter.ShouldSatisfy(Func{Assembly, bool}, Func{Assembly, TestError})" />
	#pragma warning restore CS1574
	public IExpectationConditionResult<Assembly> ShouldSatisfy(
		Func<Assembly, bool> condition,
		Func<Assembly, TestError> errorGenerator)
	{
		List<Assembly>? types = _types.Where(x => _filters.All(f => f.Applies(x))).ToList();
		if (types.Count == 0 && !_allowEmpty)
		{
			throw new EmptyDataException($"No assemblies found, that match all {_filters.Count} filters.");
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

	/// <inheritdoc cref="IExpectationFilter{Assembly}.Which(Filter{Assembly})" />
	public IExpectationFilterResult<Assembly> Which(Filter<Assembly> filter)
	{
		_filters.Add(filter);
		return this;
	}

	/// <inheritdoc />
	public IExpectationStart<Assembly> OrNone()
	{
		_allowEmpty = true;
		return this;
	}

	#endregion

	#region IExpectationFilterResult<Assembly> Members

	/// <inheritdoc cref="IExpectationFilterResult{Assembly}.And" />
	public IExpectationFilter<Assembly> And => this;

	#endregion
}
