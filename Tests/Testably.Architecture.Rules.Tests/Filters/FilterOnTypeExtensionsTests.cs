using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Filters;

public sealed partial class FilterOnTypeExtensionsTests
{
	[Fact]
	public void And_WithContraryConditions_ShouldReturnEmptyArray()
	{
		IRule rule = Expect.That.Types
			.WhichArePublic().And.WhichAreNotPublic()
			.ShouldAlwaysFail()
			.AllowEmpty();

		ITestResult result = rule.Check
			.InExecutingAssembly();

		result.ShouldNotBeViolated();
	}
}
