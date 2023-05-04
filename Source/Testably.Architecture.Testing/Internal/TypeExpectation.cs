using System;
using System.Collections.Generic;
using System.Linq;
using Testably.Architecture.Testing.Exceptions;
using Testably.Architecture.Testing.TestErrors;

namespace Testably.Architecture.Testing.Internal;

internal class TypeExpectationStart : ITypeExpectation, IExpectationFilterResult<Type>
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

	#region IExpectationFilterResult<Type> Members

	/// <inheritdoc cref="IExpectationFilterResult{Type}.And" />
	public IExpectationFilter<Type> And => this;

	#endregion

	#region ITypeExpectation Members

	#pragma warning disable CS1574
	/// <inheritdoc cref="IExpectationFilter.ShouldSatisfy(Func{Type, bool}, Func{Type, TestError})" />
	#pragma warning restore CS1574
	public IExpectationResult<Type> ShouldSatisfy(
		Func<Type, bool> condition,
		Func<Type, TestError> errorGenerator)
	{
		List<Type>? types = _types.Where(x => _filters.All(f => f.Applies(x))).ToList();
		if (types.Count == 0 && !_allowEmpty)
		{
			throw new EmptyDataException($"No types found, that match all {_filters.Count} filters.");
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

	/// <inheritdoc cref="IExpectationFilter{Type}.Which(Filter{Type})" />
	public IExpectationFilterResult<Type> Which(Filter<Type> filter)
	{
		_filters.Add(filter);
		return this;
	}

	/// <inheritdoc />
	public IExpectationStart<Type> OrNone()
	{
		_allowEmpty = true;
		return this;
	}

	#endregion
}
