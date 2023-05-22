using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Filters;

public sealed partial class AssemblyFilterExtensionsTests
{
	public sealed class MatchNameTests
	{
		[Theory]
		[InlineData("TESTABLY.Architecture.RULES", false)]
		[InlineData("Testably.Architecture.Rules", true)]
		[InlineData("T???ably.Architecture.Rules", true)]
		[InlineData("*Rules", true)]
		[InlineData("Test*", true)]
		[InlineData("test*", false)]
		[InlineData("T*s", true)]
		[InlineData("*rules", false)]
		public void WhichDoNotMatchName_CaseSensitive_ShouldReturnExpectedValue(
			string pattern, bool expectMatch)
		{
			ITestResult result = Expect.That.Assemblies
				.WhichDoNotMatchName(pattern)
				.ShouldAlwaysFail()
				.AllowEmpty()
				.Check.InAssemblyContaining<ArchitectureRuleViolatedException>();

			result.ShouldBeViolatedIf(!expectMatch);
		}

		[Theory]
		[InlineData("TESTABLY.Architecture.RULES", true)]
		[InlineData("Testably.Architecture.Rules", true)]
		[InlineData("T???ably.Architecture.Rules", true)]
		[InlineData("*rules", true)]
		[InlineData("test*", true)]
		[InlineData("test???", false)]
		[InlineData("t*s", true)]
		public void WhichDoNotMatchName_WithIgnoreCase_ShouldReturnExpectedValue(
			string pattern, bool expectMatch)
		{
			ITestResult result = Expect.That.Assemblies
				.WhichDoNotMatchName(pattern, true)
				.ShouldAlwaysFail()
				.AllowEmpty()
				.Check.InAssemblyContaining<ArchitectureRuleViolatedException>();

			result.ShouldBeViolatedIf(!expectMatch);
		}

		[Theory]
		[InlineData("TESTABLY.Architecture.RULES", false)]
		[InlineData("Testably.Architecture.Rules", true)]
		[InlineData("T???ably.Architecture.Rules", true)]
		[InlineData("*Rules", true)]
		[InlineData("Test*", true)]
		[InlineData("test*", false)]
		[InlineData("T*s", true)]
		[InlineData("*rules", false)]
		public void WhichMatchName_CaseSensitive_ShouldReturnExpectedValue(
			string pattern, bool expectMatch)
		{
			ITestResult result = Expect.That.Assemblies
				.WhichMatchName(pattern)
				.ShouldAlwaysFail()
				.AllowEmpty()
				.Check.InAssemblyContaining<ArchitectureRuleViolatedException>();

			result.ShouldBeViolatedIf(expectMatch);
		}

		[Theory]
		[InlineData("TESTABLY.Architecture.RULES", true)]
		[InlineData("Testably.Architecture.Rules", true)]
		[InlineData("T???ably.Architecture.Rules", true)]
		[InlineData("*rules", true)]
		[InlineData("test*", true)]
		[InlineData("test???", false)]
		[InlineData("t*s", true)]
		public void WhichMatchName_WithIgnoreCase_ShouldReturnExpectedValue(
			string pattern, bool expectMatch)
		{
			ITestResult result = Expect.That.Assemblies
				.WhichMatchName(pattern, true)
				.ShouldAlwaysFail()
				.AllowEmpty()
				.Check.InAssemblyContaining<ArchitectureRuleViolatedException>();

			result.ShouldBeViolatedIf(expectMatch);
		}
	}
}
