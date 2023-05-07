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
	public void Types_ShouldFilterOutTypesFromAssemblies()
	{
		List<Type> allLoadedTypes = AppDomain.CurrentDomain.GetAssemblies()
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
			.ShouldSatisfy(_ => false);

		ITestResult result = rule.Check
			.InAllLoadedAssemblies();

		result.Errors.Length.Should().BeLessThan(allLoadedTypes.Count);
		foreach (Type type in otherTypes.Skip(180))
		{
			result.Errors.Should().NotContain(e => e.ToString().Contains($"'{type.Name}'"));
		}
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
