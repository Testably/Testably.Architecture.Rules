using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Extensions;

public sealed class RuleCheckExtensionsTests
{
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void InAllLoadedAssemblies_WithPredicate_ShouldConsiderPredicateResult(
		bool predicateResult)
	{
		IRuleCheck sut = Expect.That.Assemblies
			.ShouldAlwaysFail()
			.AllowEmpty()
			.Check;

		ITestResult result = sut.InAllLoadedAssemblies(_ => predicateResult);

		result.ShouldBeViolatedIf(predicateResult);
	}

	[Theory]
	[InlineData("Testably*", false, true)]
	[InlineData("TESTABLY*", true, true)]
	[InlineData("TESTABLY*", false, false)]
	[InlineData("Foo*", false, false)]
	public void InAssembliesMatching_WithPredicate_ShouldConsiderPredicateResult(
		string match,
		bool ignoreCase,
		bool expectedResult)
	{
		IRuleCheck sut = Expect.That.Assemblies
			.ShouldAlwaysFail()
			.AllowEmpty()
			.Check;

		ITestResult result = sut.InAssembliesMatching(match, ignoreCase);

		result.ShouldBeViolatedIf(expectedResult);
	}
}
