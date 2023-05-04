using System;
using System.Collections.Generic;
using System.Linq;

namespace Testably.Architecture.Testing.Internal;

internal class TypeExpectationStart : ITypeExpectation, IFilterResult<Type>
{
	private bool _allowEmpty;
	private readonly List<Filter<Type>> _filters = new();
	private readonly TestResultBuilder<Type> _testResultBuilder;
	private readonly List<Type> _types;

	public TypeExpectationStart(IEnumerable<Type> types)
	{
		_types = types.ToList();
		_testResultBuilder = new TestResultBuilder<Type>(this);
	}

	#region IFilterResult<Type> Members

	/// <inheritdoc cref="IFilterResult{Type}.And" />
	public IFilter<Type> And => this;

	#endregion

	#region ITypeExpectation Members

	/// <inheritdoc />
	public IExpectationStart<Type> OrNone()
	{
		_allowEmpty = true;
		return this;
	}

	/// <inheritdoc cref="IFilter{Type}.Which(Filter{Type})" />
	public IFilterResult<Type> Which(Filter<Type> filter)
	{
		_filters.Add(filter);
		return this;
	}

	#pragma warning disable CS1574
	/// <inheritdoc cref="IFilter.ShouldSatisfy(Func{Type, bool}, Func{Type, TestError})" />
	#pragma warning restore CS1574
	public IRequirementResult<Type> ShouldSatisfy(
		Func<Type, bool> condition,
		Func<Type, TestError> errorGenerator)
	{
		List<Type>? types = _types.Where(x => _filters.All(f => f.Applies(x))).ToList();
		if (types.Count == 0 && !_allowEmpty)
		{
			throw new EmptyDataException(
				$"No types found, that match all {_filters.Count} filters.");
		}

		foreach (Type type in types)
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
}
