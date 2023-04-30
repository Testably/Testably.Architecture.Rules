using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Testably.Architecture.Testing.TestErrors;

namespace Testably.Architecture.Testing.Internal;

internal class AssemblyExpectation : IFilterableAssemblyExpectation
{
	private readonly List<Assembly> _assemblies;
	private readonly TestResultBuilder<AssemblyExpectation> _testResultBuilder;

	public AssemblyExpectation(IEnumerable<Assembly> assemblies)
	{
		_assemblies = assemblies.ToList();
		_testResultBuilder = new TestResultBuilder<AssemblyExpectation>(this);
	}

	#region IFilterableAssemblyExpectation Members

	/// <inheritdoc cref="IFilterableAssemblyExpectation.ShouldSatisfy(Func{Assembly, bool}, Func{Assembly, TestError})" />
	public ITestResult<IAssemblyExpectation> ShouldSatisfy(
		Func<Assembly, bool> condition,
		Func<Assembly, TestError>? errorGenerator = null)
	{
		errorGenerator ??= assembly =>
			new TestError(
				$"Assembly '{assembly.GetName().Name}' does not satisfy the required condition");
		foreach (Assembly assembly in _assemblies)
		{
			if (!condition(assembly))
			{
				TestError error = errorGenerator(assembly);
				_testResultBuilder.Add(error);
			}
		}

		return _testResultBuilder.Build();
	}

	/// <inheritdoc cref="IFilterableAssemblyExpectation.Which(Func{Assembly, bool})" />
	public IFilterableAssemblyExpectation Which(Func<Assembly, bool> predicate)
	{
		_assemblies.RemoveAll(p => !predicate(p));
		return this;
	}

	#endregion
}
