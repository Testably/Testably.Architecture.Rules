using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Filters;

public sealed partial class FilterOnTypeExtensionsTests
{
	[Fact]
	public void And_WithContraryConditions_ShouldReturnEmptyArray()
	{
		ITestResult result = Expect.That.Types
			.WhichArePublic().And.WhichAreNotPublic()
			.ShouldAlwaysFail()
			.AllowEmpty()
			.Check.InExecutingAssembly();

		result.ShouldNotBeViolated();
	}
}
