using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Reflection;
using Testably.Architecture.Testing.Internal;
using Testably.Architecture.Testing.TestErrors;
using Xunit;

namespace Testably.Architecture.Testing.Tests;

public sealed class ExtensionsForExpectationTests
{
	#region Test Setup

	public static IEnumerable<object[]> ExcludedSystemAssemblies
	{
		get
		{
			foreach (string excludedSystemAssembly in ExpectationSettings.ExcludedSystemAssemblies)
			{
				yield return new object[]
				{
					excludedSystemAssembly
				};
			}
		}
	}

	#endregion

	[Theory]
	[MemberData(nameof(ExcludedSystemAssemblies))]
	public void AllLoadedAssemblies_ShouldExcludeAssemblyStartingWith(string assemblyPrefix)
	{
		IAssemblyExpectation reference =
			Expect.That.AllLoadedAssemblies(excludeSystemAssemblies: false);
		TestError[] referenceCount = reference.ShouldSatisfy(_ => false).Errors;

		IAssemblyExpectation sut = Expect.That.AllLoadedAssemblies();

		TestError[] result = sut.ShouldSatisfy(_ => false).Errors;
#if !NCRUNCH
		referenceCount.Should().Contain(
			e => e.ToString().Contains($"'{assemblyPrefix}"),
			$"assemblies with prefix '{assemblyPrefix}' should be excluded");
#endif
		result.Should().NotContain(e => e.ToString().Contains($"'{assemblyPrefix}"));
	}

	[Fact]
	public void AllLoadedAssemblies_ShouldFilterOutSystemAssemblies()
	{
		IAssemblyExpectation reference =
			Expect.That.AllLoadedAssemblies(excludeSystemAssemblies: false);
		TestError[] referenceCount = reference.ShouldSatisfy(_ => false).Errors;

		IAssemblyExpectation sut = Expect.That.AllLoadedAssemblies();

		TestError[] result = sut.ShouldSatisfy(_ => false).Errors;
		result.Length.Should().BeLessThan(referenceCount.Length);
	}

	[Fact]
	public void AllLoadedTypes_ShouldNotBeEmpty()
	{
		ITypeExpectation sut = Expect.That.AllLoadedTypes();

		IExpectationResult<Type> result = sut.ShouldSatisfy(_ => false);

		result.Errors.Should().NotBeEmpty();
	}

	[Theory]
	[InlineData(false)]
	[InlineData(true)]
	public void AssembliesMatching_CaseMismatch_ShouldIncludeAssemblyWhenIgnoreCase(bool ignoreCase)
	{
		IExpectationStart<Assembly> sut = Expect.That
			.AssembliesMatching("testably.*", ignoreCase)
			.OrNone();

		ITestResult result = sut.ShouldSatisfy(_ => false);

		(result.Errors.Length > 0).Should().Be(ignoreCase);
	}

	[Fact]
	public void AssembliesMatching_FoundMatch_ShouldIncludeAssembly()
	{
		IAssemblyExpectation sut = Expect.That
			.AssembliesMatching("*Architecture.Testing.Tests");

		ITestResult result = sut.ShouldSatisfy(_ => false);

		result.Errors.Should().NotBeEmpty();
	}
}
