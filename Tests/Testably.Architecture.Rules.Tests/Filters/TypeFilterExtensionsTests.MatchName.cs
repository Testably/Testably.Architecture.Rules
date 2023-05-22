using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Filters;

public sealed partial class TypeFilterExtensionsTests
{
	public sealed class MatchNameTests
	{
		[Theory]
		[InlineData("TESTClass", false)]
		[InlineData("TestClass", true)]
		[InlineData("T???Class", true)]
		[InlineData("*Class", true)]
		[InlineData("Test*", true)]
		[InlineData("test*", false)]
		[InlineData("T*s", true)]
		[InlineData("*class", false)]
		public void WhichDoNotMatchName_CaseSensitive_ShouldReturnExpectedValue(
			string pattern, bool expectMatch)
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(TestClass)).And
				.WhichDoNotMatchName(pattern)
				.ShouldAlwaysFail()
				.AllowEmpty()
				.Check.InAllLoadedAssemblies();

			result.ShouldBeViolatedIf(!expectMatch);
		}

		[Theory]
		[InlineData("TESTClass", true)]
		[InlineData("TestClass", true)]
		[InlineData("t???class", true)]
		[InlineData("*class", true)]
		[InlineData("test*", true)]
		[InlineData("test???", false)]
		[InlineData("t*s", true)]
		public void WhichDoNotMatchName_WithIgnoreCase_ShouldReturnExpectedValue(
			string pattern, bool expectMatch)
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(TestClass)).And
				.WhichDoNotMatchName(pattern, true)
				.ShouldAlwaysFail()
				.AllowEmpty()
				.Check.InAllLoadedAssemblies();

			result.ShouldBeViolatedIf(!expectMatch);
		}

		[Theory]
		[InlineData("TESTClass", false)]
		[InlineData("TestClass", true)]
		[InlineData("T???Class", true)]
		[InlineData("*Class", true)]
		[InlineData("Test*", true)]
		[InlineData("test*", false)]
		[InlineData("T*s", true)]
		[InlineData("*class", false)]
		public void WhichMatchName_CaseSensitive_ShouldReturnExpectedValue(
			string pattern, bool expectMatch)
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(TestClass)).And
				.WhichMatchName(pattern)
				.ShouldAlwaysFail()
				.AllowEmpty()
				.Check.InAllLoadedAssemblies();

			result.ShouldBeViolatedIf(expectMatch);
		}

		[Theory]
		[InlineData("TESTClass", true)]
		[InlineData("TestClass", true)]
		[InlineData("t???class", true)]
		[InlineData("*class", true)]
		[InlineData("test*", true)]
		[InlineData("test???", false)]
		[InlineData("t*s", true)]
		public void WhichMatchName_WithIgnoreCase_ShouldReturnExpectedValue(
			string pattern, bool expectMatch)
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(TestClass)).And
				.WhichMatchName(pattern, true)
				.ShouldAlwaysFail()
				.AllowEmpty()
				.Check.InAllLoadedAssemblies();

			result.ShouldBeViolatedIf(expectMatch);
		}

		private class TestClass
		{
		}
	}
}
