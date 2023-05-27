using AutoFixture.Xunit2;
using FluentAssertions;
using System;
using System.Linq;
using Testably.Architecture.Rules.Internal;
using Testably.Architecture.Rules.Tests.TestHelpers;
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
	public void Constructors_ShouldApplyAllTypeFilters()
	{
		Type type1 = typeof(DummyFooClass);
		Type type2 = typeof(DummyBarClass);
		IRule rule = Expect.That.Types
			.Which(t => t == type1 || t == type2).And
			.Which(t => t == type1)
			.Constructors
			.ShouldSatisfy(_ => false);

		ITestResult result = rule.Check
			.InAllLoadedAssemblies();

		result.Errors.Length.Should().Be(2);
		result.Errors.Should().AllBeOfType<ConstructorTestError>();
		result.Errors.Should()
			.Contain(e => ((ConstructorTestError)e).Constructor.DeclaringType == type1);
		result.Errors.Should()
			.NotContain(e => ((ConstructorTestError)e).Constructor.DeclaringType == type2);
	}

	[Fact]
	public void Constructors_ShouldFilterOutConstructorsFromTypes()
	{
		Type type1 = typeof(DummyFooClass);
		Type type2 = typeof(DummyBarClass);
		IRule rule = Expect.That.Types
			.Which(t => t == type1 || t == type2)
			.Constructors
			.ShouldSatisfy(_ => false);

		ITestResult result = rule.Check
			.InAllLoadedAssemblies();

		result.Errors.Length.Should().BeGreaterThan(2);
		result.Errors.Should().AllBeOfType<ConstructorTestError>();
		result.Errors.Should()
			.Contain(e => ((ConstructorTestError)e).Constructor.DeclaringType == type1);
		result.Errors.Should()
			.Contain(e => ((ConstructorTestError)e).Constructor.DeclaringType == type2);
	}

	[Theory]
	[AutoData]
	public void Constructors_ShouldIncludeTypeFilterNamesInFilterName(
		string typeFilterName1, string typeFilterName2, string assemblyFilterName)
	{
		Type type1 = typeof(DummyFooClass);
		Type type2 = typeof(DummyBarClass);
		IRule rule = Expect.That.Types
			.Which(t => t == type1 || t == type2, typeFilterName1).And
			.Which(_ => false, typeFilterName2)
			.Constructors
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
	public void Events_ShouldApplyAllTypeFilters()
	{
		Type type1 = typeof(DummyFooClass);
		Type type2 = typeof(DummyBarClass);
		string expectedEventName1 = $"'{nameof(DummyFooClass.DummyFooEvent1)}'";
		string expectedEventName2 = $"'{nameof(DummyBarClass.DummyBarEvent1)}'";
		IRule rule = Expect.That.Types
			.Which(t => t == type1 || t == type2).And
			.Which(t => t == type1)
			.Events
			.ShouldSatisfy(_ => false);

		ITestResult result = rule.Check
			.InAllLoadedAssemblies();

		result.Errors.Length.Should().Be(2);
		result.Errors.Should().Contain(e => e.ToString().Contains(expectedEventName1));
		result.Errors.Should().NotContain(e => e.ToString().Contains(expectedEventName2));
	}

	[Fact]
	public void Events_ShouldFilterOutEventsFromTypes()
	{
		Type type1 = typeof(DummyFooClass);
		Type type2 = typeof(DummyBarClass);
		string expectedEventName1 = $"'{nameof(DummyFooClass.DummyFooEvent1)}'";
		string expectedEventName2 = $"'{nameof(DummyBarClass.DummyBarEvent1)}'";
		IRule rule = Expect.That.Types
			.Which(t => t == type1 || t == type2)
			.Events
			.ShouldSatisfy(_ => false);

		ITestResult result = rule.Check
			.InAllLoadedAssemblies();

		result.Errors.Length.Should().BeGreaterThan(2);
		result.Errors.Should().Contain(e => e.ToString().Contains(expectedEventName1));
		result.Errors.Should().Contain(e => e.ToString().Contains(expectedEventName2));
	}

	[Theory]
	[AutoData]
	public void Events_ShouldIncludeTypeFilterNamesInFilterName(
		string typeFilterName1, string typeFilterName2, string assemblyFilterName)
	{
		Type type1 = typeof(DummyFooClass);
		Type type2 = typeof(DummyBarClass);
		IRule rule = Expect.That.Types
			.Which(t => t == type1 || t == type2, typeFilterName1).And
			.Which(_ => false, typeFilterName2)
			.Events
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
	public void Fields_ShouldApplyAllTypeFilters()
	{
		Type type1 = typeof(DummyFooClass);
		Type type2 = typeof(DummyBarClass);
		string expectedFieldName1 = $"'{nameof(DummyFooClass.DummyFooField1)}'";
		string expectedFieldName2 = $"'{nameof(DummyBarClass.DummyBarField1)}'";
		IRule rule = Expect.That.Types
			.Which(t => t == type1 || t == type2).And
			.Which(t => t == type1)
			.Fields
			.ShouldSatisfy(_ => false);

		ITestResult result = rule.Check
			.InAllLoadedAssemblies();

		result.Errors.Length.Should().Be(2);
		result.Errors.Should().Contain(e => e.ToString().Contains(expectedFieldName1));
		result.Errors.Should().NotContain(e => e.ToString().Contains(expectedFieldName2));
	}

	[Fact]
	public void Fields_ShouldFilterOutFieldsFromTypes()
	{
		Type type1 = typeof(DummyFooClass);
		Type type2 = typeof(DummyBarClass);
		string expectedFieldName1 = $"'{nameof(DummyFooClass.DummyFooField1)}'";
		string expectedFieldName2 = $"'{nameof(DummyBarClass.DummyBarField1)}'";
		IRule rule = Expect.That.Types
			.Which(t => t == type1 || t == type2)
			.Fields
			.ShouldSatisfy(_ => false);

		ITestResult result = rule.Check
			.InAllLoadedAssemblies();

		result.Errors.Length.Should().BeGreaterThan(2);
		result.Errors.Should().Contain(e => e.ToString().Contains(expectedFieldName1));
		result.Errors.Should().Contain(e => e.ToString().Contains(expectedFieldName2));
	}

	[Theory]
	[AutoData]
	public void Fields_ShouldIncludeTypeFilterNamesInFilterName(
		string typeFilterName1, string typeFilterName2, string assemblyFilterName)
	{
		Type type1 = typeof(DummyFooClass);
		Type type2 = typeof(DummyBarClass);
		IRule rule = Expect.That.Types
			.Which(t => t == type1 || t == type2, typeFilterName1).And
			.Which(_ => false, typeFilterName2)
			.Fields
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
		Type type1 = typeof(DummyFooClass);
		Type type2 = typeof(DummyBarClass);
		string expectedMethodName1 = $"'{nameof(DummyFooClass.DummyFooMethod1)}'";
		string expectedMethodName2 = $"'{nameof(DummyBarClass.DummyBarMethod1)}'";
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
		Type type1 = typeof(DummyFooClass);
		Type type2 = typeof(DummyBarClass);
		string expectedMethodName1 = $"'{nameof(DummyFooClass.DummyFooMethod1)}'";
		string expectedMethodName2 = $"'{nameof(DummyBarClass.DummyBarMethod1)}'";
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
		Type type1 = typeof(DummyFooClass);
		Type type2 = typeof(DummyBarClass);
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
	public void Properties_ShouldApplyAllTypeFilters()
	{
		Type type1 = typeof(DummyFooClass);
		Type type2 = typeof(DummyBarClass);
		string expectedPropertyName1 = $"'{nameof(DummyFooClass.DummyFooProperty1)}'";
		string expectedPropertyName2 = $"'{nameof(DummyBarClass.DummyBarProperty1)}'";
		IRule rule = Expect.That.Types
			.Which(t => t == type1 || t == type2).And
			.Which(t => t == type1)
			.Properties
			.ShouldSatisfy(_ => false);

		ITestResult result = rule.Check
			.InAllLoadedAssemblies();

		result.Errors.Length.Should().Be(3);
		result.Errors.Should().Contain(e => e.ToString().Contains(expectedPropertyName1));
		result.Errors.Should().NotContain(e => e.ToString().Contains(expectedPropertyName2));
	}

	[Fact]
	public void Properties_ShouldFilterOutPropertiesFromTypes()
	{
		Type type1 = typeof(DummyFooClass);
		Type type2 = typeof(DummyBarClass);
		string expectedPropertyName1 = $"'{nameof(DummyFooClass.DummyFooProperty1)}'";
		string expectedPropertyName2 = $"'{nameof(DummyBarClass.DummyBarProperty1)}'";
		IRule rule = Expect.That.Types
			.Which(t => t == type1 || t == type2)
			.Properties
			.ShouldSatisfy(_ => false);

		ITestResult result = rule.Check
			.InAllLoadedAssemblies();

		result.Errors.Length.Should().BeGreaterThan(3);
		result.Errors.Should().Contain(e => e.ToString().Contains(expectedPropertyName1));
		result.Errors.Should().Contain(e => e.ToString().Contains(expectedPropertyName2));
	}

	[Theory]
	[AutoData]
	public void Properties_ShouldIncludeTypeFilterNamesInFilterName(
		string typeFilterName1, string typeFilterName2, string assemblyFilterName)
	{
		Type type1 = typeof(DummyFooClass);
		Type type2 = typeof(DummyBarClass);
		IRule rule = Expect.That.Types
			.Which(t => t == type1 || t == type2, typeFilterName1).And
			.Which(_ => false, typeFilterName2)
			.Properties
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
		Type type = typeof(DummyFooClass);
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
		Type type = typeof(DummyFooClass);
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
		Type type = typeof(DummyFooClass);
		IRule rule = Expect.That.Types
			.Which(t => t == type)
			.ShouldSatisfy(Requirement.ForType(_ => true, _ => error));

		ITestResult result = rule.Check
			.In(type.Assembly);

		result.Errors.Should().BeEmpty();
	}

	[Theory]
	[AutoData]
	public void ToString_ShouldCombineFilters(string filter1, string filter2)
	{
		IRule rule = Expect.That.Types
			.Which(_ => true, filter1).And
			.Which(_ => true, filter2)
			.ShouldSatisfy(_ => true);

		rule.ToString().Should().Be($"{filter1} and {filter2}");
	}

	[Fact]
	public void Which_ShouldFilterOutTypes()
	{
		Type type = typeof(DummyFooClass);
		int allTypesCount = type.Assembly.GetTypes().Length;

		IRule rule = Expect.That.Types
			.Which(p => p.Name.StartsWith(nameof(TypeRuleTests)), "foo")
			.ShouldSatisfy(Requirement.ForType(_ => false));

		ITestResult result = rule.Check
			.In(type.Assembly);

		result.Errors.Length.Should().BeLessThan(allTypesCount);
		result.Errors.Should().OnlyContain(e => e.ToString().Contains($"'{nameof(TypeRuleTests)}"));
	}
}
