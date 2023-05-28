using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Filters;

public sealed partial class FieldFilterExtensionsTests
{
	public sealed class NameMatchTests
	{
		[Theory]
		[InlineData("TESTField", false)]
		[InlineData("TestField", true)]
		[InlineData("T???Field", true)]
		[InlineData("*Field", true)]
		[InlineData("Test*", true)]
		[InlineData("test*", false)]
		[InlineData("T*d", true)]
		[InlineData("*field", false)]
		public void WhichNameMatches_CaseSensitive_ShouldReturnExpectedValue(
			string pattern, bool expectMatch)
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(TestClass)).And
				.Which(Have.Field.WhichNameMatches(pattern))
				.ShouldAlwaysFail()
				.AllowEmpty()
				.Check.InAllLoadedAssemblies();

			result.ShouldBeViolatedIf(expectMatch);
		}

		[Theory]
		[InlineData("TESTField", true)]
		[InlineData("TestField", true)]
		[InlineData("t???field", true)]
		[InlineData("*field", true)]
		[InlineData("test*", true)]
		[InlineData("test???", false)]
		[InlineData("t*d", true)]
		public void WhichNameMatches_WithIgnoreCase_ShouldReturnExpectedValue(
			string pattern, bool expectMatch)
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(TestClass)).And
				.Which(Have.Field.WhichNameMatches(pattern, true))
				.ShouldAlwaysFail()
				.AllowEmpty()
				.Check.InAllLoadedAssemblies();

			result.ShouldBeViolatedIf(expectMatch);
		}

		private class TestClass
		{
			#pragma warning disable CS0414
			public int TestField = 1;
			#pragma warning restore CS0414
		}
	}
}
