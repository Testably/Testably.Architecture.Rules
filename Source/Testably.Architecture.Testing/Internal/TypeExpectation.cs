using System;
using System.Collections.Generic;
using System.Linq;
using Testably.Architecture.Testing.TestErrors;

namespace Testably.Architecture.Testing.Internal;

internal class TypeExpectation : IOptionallyFilterableTypeExpectation, IFilteredTypeExpectation
{
	private readonly TestResultBuilder<TypeExpectation> _testResultBuilder;
	private readonly List<Type> _types;
	private readonly List<TypeFilter> _filters = new();

	public TypeExpectation(IEnumerable<Type> types)
	{
		_types = types.ToList();
		_testResultBuilder = new TestResultBuilder<TypeExpectation>(this);
	}

	#region IFilterableTypeExpectation Members

	#pragma warning disable CS1574
	/// <inheritdoc cref="IFilterableTypeExpectation.ShouldSatisfy(Func{Type, bool}, Func{Type, TestError})" />
	#pragma warning restore CS1574
	public ITestResult<ITypeExpectation> ShouldSatisfy(
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

	/// <inheritdoc cref="IFilterableTypeExpectation.Which(TypeFilter)" />
	public IFilteredTypeExpectation Which(TypeFilter filter)
	{
		_filters.Add(filter);
		return this;
	}

	#endregion

	/// <inheritdoc cref="IFilteredTypeExpectation.And" />
	public IFilterableTypeExpectation And => this;
}
