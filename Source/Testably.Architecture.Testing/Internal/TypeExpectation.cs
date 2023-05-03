using System;
using System.Collections.Generic;
using System.Linq;
using Testably.Architecture.Testing.TestErrors;

namespace Testably.Architecture.Testing.Internal;

internal class TypeExpectationStart : IExpectationStart<Type>, IFilteredTypeExpectation
{
	private readonly TestResultBuilder<TypeExpectationStart> _testResultBuilder;
	private readonly List<Type> _types;
	private readonly List<Filter<Type>> _filters = new();

	public TypeExpectationStart(IEnumerable<Type> types)
	{
		_types = types.ToList();
		_testResultBuilder = new TestResultBuilder<TypeExpectationStart>(this);
	}

	#region IFilterableTypeExpectation Members

	#pragma warning disable CS1574
	/// <inheritdoc cref="IExpectationFilter.ShouldSatisfy(Func{Type, bool}, Func{Type, TestError})" />
	#pragma warning restore CS1574
	public ITestResult<IExpectationCondition<Type>> ShouldSatisfy(
		Func<Type, bool> condition,
		Func<Type, TestError> errorGenerator)
	{
		foreach (Type type in _types.Where(x => _filters.All(f => f.Applies(x))))
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
	public IFilteredTypeExpectation Which(Filter<Type> filter)
	{
		_filters.Add(filter);
		return this;
	}

	#endregion

	/// <inheritdoc cref="IFilteredTypeExpectation.And" />
	public IExpectationFilter<Type> And => this;
}
