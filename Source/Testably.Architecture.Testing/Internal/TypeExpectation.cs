using System;
using System.Collections.Generic;
using System.Linq;
using Testably.Architecture.Testing.TestErrors;

namespace Testably.Architecture.Testing.Internal;

internal class TypeExpectation : IFilterableTypeExpectation
{
	private readonly TestResultBuilder<TypeExpectation> _testResultBuilder;
	private readonly List<Type> _types;

	public TypeExpectation(IEnumerable<Type> types)
	{
		_types = types.ToList();
		_testResultBuilder = new TestResultBuilder<TypeExpectation>(this);
	}

	#region IFilterableTypeExpectation Members

	/// <inheritdoc cref="IFilterableTypeExpectation.Which(Func{Type, bool})" />
	public IFilterableTypeExpectation Which(Func<Type, bool> predicate)
	{
		_types.RemoveAll(p => !predicate(p));
		return this;
	}

	#pragma warning disable CS1574
	/// <inheritdoc cref="IFilterableTypeExpectation.ShouldSatisfy(Func{Type, bool}, Func{Type, TestError}?)" />
	#pragma warning restore CS1574
	public ITestResult<ITypeExpectation> ShouldSatisfy(
		Func<Type, bool> condition,
		Func<Type, TestError>? errorGenerator = null)
	{
		errorGenerator ??= p =>
			new TestError($"Type '{p.Name}' does not satisfy the required condition");
		foreach (Type type in _types)
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
