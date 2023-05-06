using AutoFixture.Xunit2;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Filters;

public sealed class FilterOnAssemblyExtensionsTests
{
	[Fact]
	public void And_WithContraryConditions_ShouldReturnEmptyArray()
	{
		IRule rule = Expect.That.Assemblies
			.Which(_ => true)
			.And.Which(_ => false)
			.ShouldAlwaysFail()
			.AllowEmpty();

		ITestResult result = rule.Check
			.InExecutingAssembly();

		result.ShouldNotBeViolated();
	}

	[Theory]
	[InlineData(false)]
	[InlineData(true)]
	public void Which_WithExpression_ShouldConsiderPredicateResult(bool predicateResult)
	{
		IAssemblyFilterResult sut = Expect.That.Assemblies
			.Which(_ => predicateResult);

		ITestResult result = sut
			.ShouldAlwaysFail()
			.AllowEmpty()
			.Check.InAllLoadedAssemblies();

		result.ShouldBeViolatedIf(predicateResult);
	}

	[Theory]
	[InlineAutoData(false)]
	[InlineAutoData(true)]
	public void Which_WithName_ShouldConsiderPredicateResult(bool predicateResult, string name)
	{
		IAssemblyFilterResult sut = Expect.That.Assemblies
			.Which(_ => predicateResult, name);

		ITestResult result = sut
			.ShouldAlwaysFail()
			.AllowEmpty()
			.Check.InAllLoadedAssemblies();

		result.ShouldBeViolatedIf(predicateResult);
	}
}
