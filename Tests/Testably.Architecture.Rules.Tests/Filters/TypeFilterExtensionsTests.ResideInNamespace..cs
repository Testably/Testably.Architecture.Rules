using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Filters;

public sealed partial class TypeFilterExtensionsTests
{
	public sealed class ResideInNamespaceTests
	{
		[Theory]
		[InlineData("TESTABLY.architecture.RULES.tests.FILTERS", false)]
		[InlineData("Testably.Architecture.Rules.Tests.Filters", true)]
		[InlineData("T???????.Architecture.Rules.Tests.Filters", true)]
		[InlineData("*Filters", true)]
		[InlineData("Test*", true)]
		[InlineData("test*", false)]
		[InlineData("T*s", true)]
		[InlineData("*FILTERS", false)]
		public void WhichDoNotResideInNamespace_CaseSensitive_ShouldReturnExpectedValue(
			string pattern, bool expectMatch)
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(TypeFilterExtensionsTests)).And
				.WhichDoNotResideInNamespace(pattern)
				.ShouldAlwaysFail()
				.AllowEmpty()
				.Check.InAllLoadedAssemblies();

			result.ShouldBeViolatedIf(!expectMatch);
		}

		[Theory]
		[InlineData("TESTABLY.architecture.RULES.tests.FILTERS", true)]
		[InlineData("Testably.Architecture.Rules.Tests.Filters", true)]
		[InlineData("T???????.Architecture.Rules.Tests.Filters", true)]
		[InlineData("*Filters", true)]
		[InlineData("Test*", true)]
		[InlineData("test???", false)]
		[InlineData("T*s", true)]
		public void WhichDoNotResideInNamespace_WithIgnoreCase_ShouldReturnExpectedValue(
			string pattern, bool expectMatch)
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(TypeFilterExtensionsTests)).And
				.WhichDoNotResideInNamespace(pattern, true)
				.ShouldAlwaysFail()
				.AllowEmpty()
				.Check.InAllLoadedAssemblies();

			result.ShouldBeViolatedIf(!expectMatch);
		}

		[Theory]
		[InlineData("TESTABLY.architecture.RULES.tests.FILTERS", false)]
		[InlineData("Testably.Architecture.Rules.Tests.Filters", true)]
		[InlineData("T???????.Architecture.Rules.Tests.Filters", true)]
		[InlineData("*Filters", true)]
		[InlineData("Test*", true)]
		[InlineData("test*", false)]
		[InlineData("T*s", true)]
		[InlineData("*FILTERS", false)]
		public void WhichResideInNamespace_CaseSensitive_ShouldReturnExpectedValue(
			string pattern, bool expectMatch)
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(TypeFilterExtensionsTests)).And
				.WhichResideInNamespace(pattern)
				.ShouldAlwaysFail()
				.AllowEmpty()
				.Check.InAllLoadedAssemblies();

			result.ShouldBeViolatedIf(expectMatch);
		}

		[Theory]
		[InlineData("TESTABLY.architecture.RULES.tests.FILTERS", true)]
		[InlineData("Testably.Architecture.Rules.Tests.Filters", true)]
		[InlineData("T???????.Architecture.Rules.Tests.Filters", true)]
		[InlineData("*Filters", true)]
		[InlineData("Test*", true)]
		[InlineData("test???", false)]
		[InlineData("T*s", true)]
		public void WhichResideInNamespace_WithIgnoreCase_ShouldReturnExpectedValue(
			string pattern, bool expectMatch)
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(TypeFilterExtensionsTests)).And
				.WhichResideInNamespace(pattern, true)
				.ShouldAlwaysFail()
				.AllowEmpty()
				.Check.InAllLoadedAssemblies();

			result.ShouldBeViolatedIf(expectMatch);
		}
	}
}
