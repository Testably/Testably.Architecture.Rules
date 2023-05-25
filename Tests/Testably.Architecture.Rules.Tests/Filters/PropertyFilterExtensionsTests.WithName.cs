using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Filters;

public sealed partial class PropertyFilterExtensionsTests
{
	public sealed class WithNameTests
	{
		[Theory]
		[InlineData("TESTProperty", false)]
		[InlineData("TestProperty", true)]
		[InlineData("T???Property", true)]
		[InlineData("*Property", true)]
		[InlineData("Test*", true)]
		[InlineData("test*", false)]
		[InlineData("T*y", true)]
		[InlineData("*property", false)]
		public void WhichMatchName_CaseSensitive_ShouldReturnExpectedValue(
			string pattern, bool expectMatch)
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(TestClass)).And
				.Which(Have.Property.WithName(pattern))
				.ShouldAlwaysFail()
				.AllowEmpty()
				.Check.InAllLoadedAssemblies();

			result.ShouldBeViolatedIf(expectMatch);
		}

		[Theory]
		[InlineData("TESTProperty", true)]
		[InlineData("TestProperty", true)]
		[InlineData("t???property", true)]
		[InlineData("*property", true)]
		[InlineData("test*", true)]
		[InlineData("test???", false)]
		[InlineData("t*y", true)]
		public void WhichMatchName_WithIgnoreCase_ShouldReturnExpectedValue(
			string pattern, bool expectMatch)
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(TestClass)).And
				.Which(Have.Property.WithName(pattern, true))
				.ShouldAlwaysFail()
				.AllowEmpty()
				.Check.InAllLoadedAssemblies();

			result.ShouldBeViolatedIf(expectMatch);
		}

		private class TestClass
		{
			// ReSharper disable once UnusedMember.Local
			public int TestProperty { get; set; }
		}
	}
}
