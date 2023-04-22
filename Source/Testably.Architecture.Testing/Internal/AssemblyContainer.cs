using System;
using System.Linq;
using System.Reflection;

namespace Testably.Architecture.Testing.Internal;

internal class AssemblyContainer : IProjectExpectation
{
	private readonly Assembly _assembly;

	public AssemblyContainer(Assembly assembly)
	{
		_assembly = assembly;
	}

	#region IProjectExpectation Members

	/// <inheritdoc />
	public ITestResult ShouldOnlyHaveDependenciesThatSatisfy(
		Func<AssemblyName, bool> condition,
		Func<AssemblyName, TestError>? errorGenerator = null)
	{
		errorGenerator ??= a =>
			new TestError($"Dependency '{a.Name} does not satisfy the required condition");
		TestError[]? errors = _assembly.GetReferencedAssemblies()
			.Where(x => !condition(x))
			.Select(errorGenerator)
			.ToArray();
		return new TestResult(errors);
	}

	#endregion
}
