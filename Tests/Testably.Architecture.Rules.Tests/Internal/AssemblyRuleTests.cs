using AutoFixture.Xunit2;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Testably.Architecture.Rules.Internal;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Internal;

public sealed class AssemblyRuleTests
{
	[Fact]
	public void ShouldSatisfy_DefaultError_ShouldIncludeAssemblyName()
	{
		Assembly assembly = Assembly.GetExecutingAssembly();
		string expectedAssemblyName = $"'{assembly.GetName().Name}'";
		IRule rule = Expect.That.Assemblies
			.ShouldSatisfy(_ => false);

		ITestResult result = rule.Check
			.In(assembly);

		TestError error = result.Errors.Single();
		error.ToString().Should().Contain(expectedAssemblyName);
	}

	[Theory]
	[AutoData]
	public void ShouldSatisfy_False_ShouldIncludeError(TestError error)
	{
		IRule rule = Expect.That.Assemblies
			.ShouldSatisfy(Requirement.ForAssembly(_ => false, _ => error));

		ITestResult result = rule.Check
			.InExecutingAssembly();

		result.Errors.Should().NotBeEmpty();
		result.Errors.Single().Should().Be(error);
	}

	[Theory]
	[AutoData]
	public void ShouldSatisfy_True_ShouldNotIncludeError(TestError error)
	{
		IRule rule = Expect.That.Assemblies
			.ShouldSatisfy(Requirement.ForAssembly(_ => true, _ => error));

		ITestResult result = rule.Check
			.InExecutingAssembly();

		result.Errors.Should().BeEmpty();
	}

	[Fact]
	public void Types_ShouldApplyAllAssemblyFilters()
	{
		TestDataProvider provider = new(
			AppDomain.CurrentDomain.GetAssemblies());
		List<Type> allLoadedTypes = provider.GetAssemblies()
			.SelectMany(a => a.GetTypes())
			.ToList();
		Assembly assembly1 = typeof(TypeRuleTests).Assembly;
		Assembly assembly2 = typeof(TypeRule).Assembly;
		List<Type> otherTypes = allLoadedTypes
			.Where(t => t.Assembly != assembly1 && t.Assembly != assembly2)
			.ToList();
		IRule rule = Expect.That.Assemblies
			.Which(a => a == assembly1 || a == assembly2).And
			.Which(a => a == assembly1)
			.Types
			.ShouldSatisfy(Requirement.ForType(
				_ => false,
				t => new TypeTestError(t, "")));

		ITestResult result = rule.Check
			.InAllLoadedAssemblies();

		result.Errors.Length.Should().BeLessThan(allLoadedTypes.Count);
		foreach (TestError error in result.Errors)
		{
			error.Should().BeOfType<TypeTestError>()
				.Which.Type.Assembly.FullName.Should().Be(assembly1.FullName);
		}

		foreach (Type type in otherTypes)
		{
			result.Errors.Should().NotContain(e => e.ToString().Contains($"'{type.FullName}'"));
		}
	}

	[Fact]
	public void Types_ShouldFilterOutTypesFromAssemblies()
	{
		TestDataProvider provider = new(
			AppDomain.CurrentDomain.GetAssemblies());
		List<Type> allLoadedTypes = provider.GetAssemblies()
			.SelectMany(a => a.GetTypes())
			.ToList();
		Assembly assembly1 = typeof(TypeRuleTests).Assembly;
		Assembly assembly2 = typeof(TypeRule).Assembly;
		List<Type> otherTypes = allLoadedTypes
			.Where(t => t.Assembly != assembly1 && t.Assembly != assembly2)
			.ToList();
		IRule rule = Expect.That.Assemblies
			.Which(a => a == assembly1 || a == assembly2)
			.Types
			.ShouldSatisfy(Requirement.ForType(
				_ => false,
				t => new TypeTestError(t, "")));

		ITestResult result = rule.Check
			.InAllLoadedAssemblies();

		result.Errors.Length.Should().BeLessThan(allLoadedTypes.Count);
		foreach (TestError error in result.Errors)
		{
			error.Should().BeOfType<TypeTestError>()
				.Which.Type.Assembly.FullName.Should()
				.Match(f => f == assembly1.FullName ||
				            f == assembly2.FullName);
		}

		foreach (Type type in otherTypes)
		{
			result.Errors.Should().NotContain(e => e.ToString().Contains($"'{type.FullName}'"));
		}
	}

	[Theory]
	[AutoData]
	public void Types_ShouldIncludeAssemblyFilterNamesInFilterName(
		string assemblyFilterName1,
		string assemblyFilterName2,
		string typeFilterName)
	{
		IRule rule = Expect.That.Assemblies
			.Which(_ => true, assemblyFilterName1).And
			.Which(_ => true, assemblyFilterName2)
			.Types
			.Which(_ => false, typeFilterName)
			.ShouldSatisfy(_ => false);
		string expectedAssemblyFilters = $"{assemblyFilterName1}, {assemblyFilterName2}";

		ITestResult result = rule.Check
			.InAllLoadedAssemblies();

		result.Errors.Length.Should().Be(1);
		result.Errors[0].Should().BeOfType<EmptySourceTestError>()
			.Which.ToString().Should()
			.Contain(expectedAssemblyFilters).And.Contain(typeFilterName);
	}

	[Fact]
	public void Which_ShouldFilterOutAssemblies()
	{
		int allAssembliesCount = AppDomain.CurrentDomain.GetAssemblies().Length;

		IRule rule = Expect.That.Assemblies
			.Which(p => p.GetName().Name?.StartsWith("Testably") == true, "foo")
			.ShouldSatisfy(Requirement.ForAssembly(_ => false));

		ITestResult result = rule.Check
			.InExecutingAssembly();

		result.Errors.Length.Should().BeLessThan(allAssembliesCount);
		result.Errors.Should().OnlyContain(e => e.ToString().Contains("'Testably"));
	}
}
