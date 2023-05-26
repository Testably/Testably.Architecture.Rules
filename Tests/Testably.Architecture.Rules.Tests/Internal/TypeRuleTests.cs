using AutoFixture.Xunit2;
using FluentAssertions;
using System;
using System.Linq;
using Testably.Architecture.Rules.Internal;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Internal;

public sealed class TypeRuleTests
{
	[Fact]
	public void Assemblies_ShouldApplyAllTypeFilters()
	{
		Type type1 = typeof(TypeRuleTests);
		Type type2 = typeof(TypeRule);
		string expectedAssemblyName1 = $"'{type1.Assembly.GetName().Name}'";
		string expectedAssemblyName2 = $"'{type2.Assembly.GetName().Name}'";
		IRule rule = Expect.That.Types
			.Which(t => t == type1 || t == type2).And
			.Which(t => t == type1)
			.Assemblies
			.ShouldSatisfy(_ => false);

		ITestResult result = rule.Check
			.InAllLoadedAssemblies();

		result.Errors.Length.Should().Be(1);
		result.Errors.Should().Contain(e => e.ToString().Contains(expectedAssemblyName1));
		result.Errors.Should().NotContain(e => e.ToString().Contains(expectedAssemblyName2));
	}

	[Fact]
	public void Assemblies_ShouldFilterOutAssembliesFromTypes()
	{
		Type type1 = typeof(TypeRuleTests);
		Type type2 = typeof(TypeRule);
		string expectedAssemblyName1 = $"'{type1.Assembly.GetName().Name}'";
		string expectedAssemblyName2 = $"'{type2.Assembly.GetName().Name}'";
		IRule rule = Expect.That.Types
			.Which(t => t == type1 || t == type2)
			.Assemblies
			.ShouldSatisfy(_ => false);

		ITestResult result = rule.Check
			.InAllLoadedAssemblies();

		result.Errors.Length.Should().Be(2);
		result.Errors.Should().Contain(e => e.ToString().Contains(expectedAssemblyName1));
		result.Errors.Should().Contain(e => e.ToString().Contains(expectedAssemblyName2));
	}

	[Theory]
	[AutoData]
	public void Assemblies_ShouldIncludeTypeFilterNamesInFilterName(
		string typeFilterName1, string typeFilterName2, string assemblyFilterName)
	{
		Type type1 = typeof(TypeRuleTests);
		Type type2 = typeof(TypeRule);
		IRule rule = Expect.That.Types
			.Which(t => t == type1 || t == type2, typeFilterName1).And
			.Which(_ => false, typeFilterName2)
			.Assemblies
			.Which(_ => false, assemblyFilterName)
			.ShouldSatisfy(_ => true);
		string expectedTypeFilters = $"{typeFilterName1}, {typeFilterName2}";

		ITestResult result = rule.Check
			.InAllLoadedAssemblies();

		result.Errors.Length.Should().Be(1);
		result.Errors[0].Should().BeOfType<EmptySourceTestError>()
			.Which.ToString().Should()
			.Contain(expectedTypeFilters).And.Contain(assemblyFilterName);
	}

	[Fact]
	public void Methods_ShouldApplyAllTypeFilters()
	{
		Type type1 = typeof(Dummy);
		Type type2 = typeof(TypeRuleTests);
		string expectedMethodName1 = $"'{nameof(Dummy.Method1)}'";
		string expectedMethodName2 = $"'{nameof(Methods_ShouldApplyAllTypeFilters)}'";
		IRule rule = Expect.That.Types
			.Which(t => t == type1 || t == type2).And
			.Which(t => t == type1)
			.Methods
			.ShouldSatisfy(_ => false);

		ITestResult result = rule.Check
			.InAllLoadedAssemblies();

		result.Errors.Length.Should().Be(2);
		result.Errors.Should().Contain(e => e.ToString().Contains(expectedMethodName1));
		result.Errors.Should().NotContain(e => e.ToString().Contains(expectedMethodName2));
	}

	[Fact]
	public void Methods_ShouldFilterOutMethodsFromTypes()
	{
		Type type1 = typeof(Dummy);
		Type type2 = typeof(TypeRuleTests);
		string expectedMethodName1 = $"'{nameof(Dummy.Method1)}'";
		string expectedMethodName2 = $"'{nameof(Methods_ShouldApplyAllTypeFilters)}'";
		IRule rule = Expect.That.Types
			.Which(t => t == type1 || t == type2)
			.Methods
			.ShouldSatisfy(_ => false);

		ITestResult result = rule.Check
			.InAllLoadedAssemblies();

		result.Errors.Length.Should().BeGreaterThan(2);
		result.Errors.Should().Contain(e => e.ToString().Contains(expectedMethodName1));
		result.Errors.Should().Contain(e => e.ToString().Contains(expectedMethodName2));
	}

	[Theory]
	[AutoData]
	public void Methods_ShouldIncludeTypeFilterNamesInFilterName(
		string typeFilterName1, string typeFilterName2, string assemblyFilterName)
	{
		Type type1 = typeof(TypeRuleTests);
		Type type2 = typeof(TypeRule);
		IRule rule = Expect.That.Types
			.Which(t => t == type1 || t == type2, typeFilterName1).And
			.Which(_ => false, typeFilterName2)
			.Methods
			.Which(_ => false, assemblyFilterName)
			.ShouldSatisfy(_ => true);
		string expectedTypeFilters = $"{typeFilterName1}, {typeFilterName2}";

		ITestResult result = rule.Check
			.InAllLoadedAssemblies();

		result.Errors.Length.Should().Be(1);
		result.Errors[0].Should().BeOfType<EmptySourceTestError>()
			.Which.ToString().Should()
			.Contain(expectedTypeFilters).And.Contain(assemblyFilterName);
	}

	[Fact]
	public void ShouldSatisfy_DefaultError_ShouldIncludeTypeName()
	{
		Type type = typeof(TypeRuleTests);
		string expectedTypeName = $"'{type.FullName}'";
		IRule rule = Expect.That.Types
			.Which(t => t == type)
			.ShouldSatisfy(_ => false);

		ITestResult result = rule.Check
			.In(type.Assembly);

		TestError error = result.Errors.Single();
		error.ToString().Should().Contain(expectedTypeName);
	}

	[Theory]
	[AutoData]
	public void ShouldSatisfy_False_ShouldIncludeError(TestError error)
	{
		Type type = typeof(TypeRuleTests);
		IRule rule = Expect.That.Types
			.Which(t => t == type)
			.ShouldSatisfy(Requirement.ForType(_ => false, _ => error));

		ITestResult result = rule.Check
			.In(type.Assembly);

		result.Errors.Should().NotBeEmpty();
		result.Errors.Single().Should().Be(error);
	}

	[Theory]
	[AutoData]
	public void ShouldSatisfy_True_ShouldNotIncludeError(TestError error)
	{
		Type type = typeof(TypeRuleTests);
		IRule rule = Expect.That.Types
			.Which(t => t == type)
			.ShouldSatisfy(Requirement.ForType(_ => true, _ => error));

		ITestResult result = rule.Check
			.In(type.Assembly);

		result.Errors.Should().BeEmpty();
	}

	[Fact]
	public void Which_ShouldFilterOutTypes()
	{
		Type type = typeof(TypeRuleTests);
		int allTypesCount = type.Assembly.GetTypes().Length;

		IRule rule = Expect.That.Types
			.Which(p => p.Name.StartsWith(nameof(TypeRuleTests)), "foo")
			.ShouldSatisfy(Requirement.ForType(_ => false));

		ITestResult result = rule.Check
			.In(type.Assembly);

		result.Errors.Length.Should().BeLessThan(allTypesCount);
		result.Errors.Should().OnlyContain(e => e.ToString().Contains($"'{nameof(TypeRuleTests)}"));
	}

	private class Dummy
	{
		public void Method1()
		{
		}

		// ReSharper disable once UnusedMember.Local
		public void Method2()
		{
		}
	}
}
