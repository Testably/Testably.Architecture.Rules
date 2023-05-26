using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Filters;

public sealed partial class EventFilterExtensionsTests
{
	public sealed class WithNameTests
	{
		[Theory]
		[InlineData("TESTEvent", false)]
		[InlineData("TestEvent", true)]
		[InlineData("T???Event", true)]
		[InlineData("*Event", true)]
		[InlineData("Test*", true)]
		[InlineData("test*", false)]
		[InlineData("T*t", true)]
		[InlineData("*event", false)]
		public void WhichMatchName_CaseSensitive_ShouldReturnExpectedValue(
			string pattern, bool expectMatch)
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(TestClass)).And
				.Which(Have.Event.WithName(pattern))
				.ShouldAlwaysFail()
				.AllowEmpty()
				.Check.InAllLoadedAssemblies();

			result.ShouldBeViolatedIf(expectMatch);
		}

		[Theory]
		[InlineData("TESTEvent", true)]
		[InlineData("TestEvent", true)]
		[InlineData("t???event", true)]
		[InlineData("*event", true)]
		[InlineData("test*", true)]
		[InlineData("test???", false)]
		[InlineData("t*t", true)]
		public void WhichMatchName_WithIgnoreCase_ShouldReturnExpectedValue(
			string pattern, bool expectMatch)
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(TestClass)).And
				.Which(Have.Event.WithName(pattern, true))
				.ShouldAlwaysFail()
				.AllowEmpty()
				.Check.InAllLoadedAssemblies();

			result.ShouldBeViolatedIf(expectMatch);
		}

		private class TestClass
		{
			// ReSharper disable once EventNeverSubscribedTo.Local
			#pragma warning disable CS0067
			#pragma warning disable CS8618
			public event Dummy TestEvent;
			#pragma warning restore CS8618
			#pragma warning restore CS0067
			public delegate void Dummy();
		}
	}
}
