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
	public void Assemblies_ShouldFilterOutAssembliesFromTypes()
	{
		Type type1 = typeof(TypeRuleTests);
		Type type2 = typeof(TypeRule);
		string expectedAssemblyName1 = $"'{type1.Assembly.GetName().Name}'";
		string expectedAssemblyName2 = $"'{type1.Assembly.GetName().Name}'";
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

	[Fact]
	public void ShouldSatisfy_DefaultError_ShouldIncludeTypeName()
	{
		Type type = typeof(TypeRuleTests);
		string expectedTypeName = $"'{type.Name}'";
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
}
