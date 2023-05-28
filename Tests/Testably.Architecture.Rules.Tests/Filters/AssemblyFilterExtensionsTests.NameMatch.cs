using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Filters;

public sealed partial class AssemblyFilterExtensionsTests
{
	public sealed class NameMatchTests
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
		public void WhichNameDoesNotMatch_CaseSensitive_ShouldReturnExpectedValue(
			string pattern, bool expectMatch)
		{
			ITestResult result = Expect.That.Assemblies
				.WhichNameDoesNotMatch(pattern)
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
		public void WhichNameDoesNotMatch_WithIgnoreCase_ShouldReturnExpectedValue(
			string pattern, bool expectMatch)
		{
			ITestResult result = Expect.That.Assemblies
				.WhichNameDoesNotMatch(pattern, true)
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
		public void WhichNameMatches_CaseSensitive_ShouldReturnExpectedValue(
			string pattern, bool expectMatch)
		{
			ITestResult result = Expect.That.Assemblies
				.WhichNameMatches(pattern)
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
		public void WhichNameMatches_WithIgnoreCase_ShouldReturnExpectedValue(
			string pattern, bool expectMatch)
		{
			ITestResult result = Expect.That.Assemblies
				.WhichNameMatches(pattern, true)
				.ShouldAlwaysFail()
				.AllowEmpty()
				.Check.InAssemblyContaining<ArchitectureRuleViolatedException>();

			result.ShouldBeViolatedIf(expectMatch);
		}
	}
}
